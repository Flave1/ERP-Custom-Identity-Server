using APIGateway.Contracts.Commands.Email;
using APIGateway.Contracts.Commands.Workflow;
using APIGateway.Data;
using APIGateway.DomainObjects.Workflow;
using APIGateway.MailHandler;
using APIGateway.MailHandler.Service;
using APIGateway.Repository.Interface.Workflow;
using AutoMapper;
using GODP.APIsContinuation.DomainObjects.UserAccount; 
using GODP.APIsContinuation.Repository.Interface.Admin;
using GODPAPIs.Contracts.RequestResponse.Workflow; 
using GODPCloud.Helpers.Extensions;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Workflow
{
    public class GoForApprovalCommandHandler : IRequestHandler<GoForApprovalCommand, GoForApprovalRespObj>
    {
        private readonly IWorkflowRepository _repo;
        private readonly ILoggerService _logger;
        private readonly UserManager<cor_useraccount> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAdminRepository _adminRepo;
        private readonly RoleManager<cor_userrole> _roleManager;
        private readonly DataContext _dataContext;
        private readonly IEmailService _email;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private class InMemoryStaffObject
        {
            public int StaffId { get; set; }
            public string Email { get; set; } 
            public int Position { get; set; }
            public int StaffOfficeId { get; set; }
            public string Name { get; set; }
            public int WorkflowLevelId { get; set; } 
            public int GroupId { get; set; }
        }
        public class Groups
        { 
            public int GroupID { get; set; } 
            public List<Levels> Levels { get; set; }
        }
        public class Levels
        {
            public int GroupID { get; set; } 
            public int WorkflowLevelId { get; set; } 
            public List<SameTitle> SameTitles { get; set; }
        }
        public class SameTitle
        {
            public int GroupID { get; set; } 
            public int WorkflowLevelId { get; set; } 

            public int StaffId { get; set; }
            public string Email { get; set; }
            public int Position { get; set; }
            public int StaffOfficeId { get; set; }
            public string Name { get; set; } 
        }
        public GoForApprovalCommandHandler(
            IWorkflowRepository workflowRepository, 
            ILoggerService loggerService, 
            RoleManager<cor_userrole> roleManager,
            UserManager<cor_useraccount> userManager, 
            IHttpContextAccessor httpContextAccessor, 
            IAdminRepository adminRepository, 
            DataContext data,
            IEmailService emailService,
            IMediator mediator,
            IMapper mapper)
        {
            _repo = workflowRepository;
            _userManager = userManager;
            _logger = loggerService;
            _adminRepo = adminRepository;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
            _dataContext = data;
            _email = emailService;
            _mediator = mediator;
            _mapper = mapper;
        }
        public async Task<GoForApprovalRespObj> Handle(GoForApprovalCommand request, CancellationToken cancellationToken)
        {
            var operationList = await _repo.GetAllOperationAsync();
            var workflowflowAccessList = await _repo.GetAllWorkflowAccessAsync();
            var staffList = await _adminRepo.GetAllStaffAsync();
            var workflowLevelList = await _repo.GetAllWorkflowLevelAsync();
            var workflowLevelStaffList = await _repo.GetAllWorkflowLevelStaffAsync();
            var workflowDetailList = await _repo.GetAllWorkflowdetailsAsync();
            var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;
            var user = await _userManager.FindByIdAsync(currentUserId);
            var currentUserStaffData = staffList.FirstOrDefault(d => d.StaffId == user.StaffId);
            var validationResponse = ValidateRequest(request);
            if (!validationResponse.Status.IsSuccessful)
                return validationResponse;

            try
            {
                if(!(bool)operationList.FirstOrDefault(x => x.OperationId == request.OperationId).EnableWorkflow)
                {
                    return new GoForApprovalRespObj
                    {
                        EnableWorkflow = false,
                        HasWorkflowAccess = true,
                        ApprovalProcessStarted = false,
                        Status = new APIResponseStatus
                        {
                            IsSuccessful = true,
                            Message = new APIResponseMessage
                            {
                                FriendlyMessage = "Workflow not enabled for this company! please go ahead with process"
                            }
                        }
                    };
                }
                 
                var thisStaffWorkflowAccess = workflowflowAccessList.FirstOrDefault(x => x.OfficeAccessId == currentUserStaffData.StaffOfficeId && x.OperationId == request.OperationId);
                if (thisStaffWorkflowAccess == null)
                {
                    return new GoForApprovalRespObj
                    {
                        EnableWorkflow = true,
                        HasWorkflowAccess = false,
                        Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "There is no approval workflow setup for your access level" } }
                    };
                }
               
                var thisOperationworkflow = _dataContext.cor_workflow.FirstOrDefault(c => c.OperationId == request.OperationId); 

                var thisOperationWorkflowDetails = workflowDetailList.Where(x => x.WorkflowId == thisOperationworkflow.WorkflowId && x.OperationId == request.OperationId).OrderBy(c=>c.Position).ToList();

                var wkfLevelaccessList = new List<int>();
                foreach(var approver in thisOperationWorkflowDetails)
                {
                    wkfLevelaccessList.Add(Convert.ToInt32(approver.WorkflowLevelId));
                } 

                var StaffBindedObj = (from wkflstaff in workflowLevelStaffList
                                 join wkfl in workflowLevelList on wkflstaff.WorkflowLevelId equals wkfl.WorkflowLevelId
                                 join thisOpWkDetail in thisOperationWorkflowDetails on wkflstaff.WorkflowLevelId equals thisOpWkDetail.WorkflowLevelId
                                 where wkfLevelaccessList.Contains(wkflstaff.WorkflowLevelId)
                                 select new InMemoryStaffObject
                                 { 
                                    StaffId = wkflstaff.StaffId,
                                    WorkflowLevelId = wkfl.WorkflowLevelId
                                 }).ToList();
                List<InMemoryStaffObject> WorkflowLevelStaffObject = new List<InMemoryStaffObject>();
                if (StaffBindedObj.Count() > 0)
                {
                    
                    foreach (var staff in StaffBindedObj.OrderBy(s => s.Position))
                    {
                        var newStaffObj = new InMemoryStaffObject
                        {
                            Email = staffList.FirstOrDefault(s => s.StaffId == staff.StaffId)?.Email,
                            StaffOfficeId = staffList.FirstOrDefault(s => s.StaffId == staff.StaffId)?.AccessLevel ?? 0,
                            StaffId = staff.StaffId,
                            Position = thisOperationWorkflowDetails.FirstOrDefault(d => d.WorkflowLevelId == staff.WorkflowLevelId).Position,
                            Name = $"{staffList.FirstOrDefault(s => s.StaffId == staff.StaffId)?.FirstName} {staffList.FirstOrDefault(s => s.StaffId == staff.StaffId)?.LastName}",
                            WorkflowLevelId = staff.WorkflowLevelId
                        };
                        WorkflowLevelStaffObject.Add(newStaffObj);
                    }
                }
  
                if (StaffBindedObj.Count() < 1)
                {
                    StaffBindedObj = new List<InMemoryStaffObject>();
                    WorkflowLevelStaffObject = new List<InMemoryStaffObject>();

                    var thisOperationGrouped = workflowLevelList
                        .Where(s => thisOperationWorkflowDetails.GroupBy(w => w.WorkflowGroupId).First()
                        .Select(g => g.WorkflowGroupId).Contains(s.WorkflowGroupId))
                        .Select(c => new Groups() { GroupID = c.WorkflowGroupId }).ToList();
                    if(thisOperationGrouped.Count() < 1)
                    {
                        return new GoForApprovalRespObj
                        {
                            EnableWorkflow = true,
                            HasWorkflowAccess = true,
                            ApprovalProcessStarted = false,
                            Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "No Workflow  Group Found for this Operation" } }
                        };
                    }
                     
                    var thisOperationGroupedLevels = thisOperationWorkflowDetails
                       .Where(s => thisOperationGrouped
                       .Select(r => r.GroupID).Contains(s.WorkflowGroupId))
                       .Select(e => new Levels() { GroupID = e.WorkflowGroupId, WorkflowLevelId = e.WorkflowLevelId }).ToList();
                   
                    if (thisOperationGroupedLevels.Count() < 1)
                    {
                        return new GoForApprovalRespObj
                        {
                            EnableWorkflow = true,
                            HasWorkflowAccess = true,
                            ApprovalProcessStarted = false,
                            Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "No Workflow  Level Found for this Operation" } }
                        };
                    }

                    var titlesOfLevelsGrouped = (from a in thisOperationWorkflowDetails
                                             join b in thisOperationGroupedLevels
                                             on new
                                             {
                                                 X1 = a.WorkflowLevelId,
                                                 X2 = a.WorkflowGroupId
                                             }
                                             equals new
                                             {
                                                 X1 = b.WorkflowLevelId,
                                                 X2 = b.GroupID
                                             }
                                             join c in workflowLevelList on b.WorkflowLevelId equals c.WorkflowLevelId
                                             join d in staffList on int.Parse(c.RoleId) equals d.JobTitle
                                             orderby c.Position
                                             select new InMemoryStaffObject
                                             {
                                                 StaffId = d.StaffId,
                                                 WorkflowLevelId = b.WorkflowLevelId,
                                                 Position = a.Position,
                                                 Email = d.Email,
                                                 Name = $"{staffList.FirstOrDefault(s => s.StaffId == d.StaffId)?.FirstName} {staffList.FirstOrDefault(s => s.StaffId == d.StaffId)?.LastName}",
                                                 StaffOfficeId = d?.AccessLevel ?? 0,
                                                 GroupId = a.WorkflowGroupId
                                             }).ToList();
                    WorkflowLevelStaffObject = titlesOfLevelsGrouped;
                }
                var TaskToken = CustomToken.Generate();
                using (var _trans = await _dataContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        foreach(var target in request.TargetId)
                        { 
                            cor_workflowtask appproverTask = new cor_workflowtask();
                            foreach (var staff in WorkflowLevelStaffObject)
                            {
                                appproverTask = new cor_workflowtask();
                                appproverTask.Active = true;
                                appproverTask.Comment = request.Comment;
                                appproverTask.CompanyId = request.CompanyId;
                                appproverTask.CreatedBy = user.UserName;
                                appproverTask.CreatedOn = DateTime.Now;
                                appproverTask.DefferedExecution = request.DeferredExecution;
                                appproverTask.Deleted = false;
                                appproverTask.OperationId = request.OperationId;
                                appproverTask.StaffAccessId = staff.StaffOfficeId;
                                appproverTask.StaffEmail = staff.Email;
                                appproverTask.StaffId = staff.StaffId;
                                appproverTask.StaffRoles = string.Join(',', await _adminRepo.GetStaffRolesAsync(staff.StaffId));
                                appproverTask.TargetId = target;
                                appproverTask.WorkflowId = thisOperationworkflow.WorkflowId;
                                appproverTask.WorkflowTaskId = request.WorkflowTaskId;
                                appproverTask.ApprovalStatus = (int)ApprovalStatus.Pending;
                                appproverTask.Date = DateTime.Now;
                                appproverTask.Position = staff.Position;
                                appproverTask.WorkflowToken = TaskToken;
                                if (request.WorkflowTaskId < 1)
                                {
                                    appproverTask.WorkflowTaskStatus = (int)WorkflowTaskStatus.Created;
                                    appproverTask.ApprovalStatus = (int)ApprovalStatus.Processing;
                                    if (appproverTask.Position == 1)
                                    {
                                        appproverTask.IsMailSent = true;
                                        await SendMail(appproverTask.StaffEmail, staff.Name, appproverTask.Comment);
                                        //Send Mail...................
                                    }
                                    if (!await _repo.CreateUpdateWorkflowTaskAsync(appproverTask))
                                    {
                                        await _trans.RollbackAsync();
                                        return new GoForApprovalRespObj
                                        {
                                            EnableWorkflow = true,
                                            HasWorkflowAccess = true,
                                            ApprovalProcessStarted = false,
                                            Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Approval Process failled to start" } }
                                        };
                                    }
                                }
                            }
                        } 
                        await _trans.CommitAsync();
                        return new GoForApprovalRespObj
                        {
                            EnableWorkflow = true,
                            HasWorkflowAccess = true,
                            ApprovalProcessStarted = true, 
                            Status = new APIResponseStatus { CustomToken = TaskToken, IsSuccessful = true, Message = new APIResponseMessage { FriendlyMessage = "Approval Process started" } }
                        };
                    }
                    catch (Exception ex)
                    {
                        var errorCode = ErrorID.Generate(7);
                        await _trans.RollbackAsync();
                        _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                        return new GoForApprovalRespObj
                        {
                            EnableWorkflow = false,
                            HasWorkflowAccess = false,
                            ApprovalProcessStarted = false,
                            Status = new APIResponseStatus
                            {
                                IsSuccessful = false,
                                Message = new APIResponseMessage
                                {
                                    FriendlyMessage = "Error Occured While processing request!! Please check your technical message",
                                    TechnicalMessage = ex?.Message ?? ex?.InnerException.Message + ex?.StackTrace,
                                }
                            }
                        };
                    }
                    finally { await _trans.DisposeAsync(); }
                }
            }
            catch (Exception ex)
            {
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new GoForApprovalRespObj
                {
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Unable to process item",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }

        private GoForApprovalRespObj ValidateRequest(GoForApprovalCommand request)
        {
            if (request.StaffId <= 0) 
            {
                return new GoForApprovalRespObj
                {
                    EnableWorkflow = true,
                    HasWorkflowAccess = true,
                    ApprovalProcessStarted = false,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage { FriendlyMessage = $"Invalid Call! staffid cannot be {request.StaffId}" }
                    }
                };
            }

            if (request.OperationId <= 0)
            {
                return new GoForApprovalRespObj
                {
                    EnableWorkflow = true,
                    HasWorkflowAccess = true,
                    ApprovalProcessStarted = false,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage { FriendlyMessage = $"Invalid Call! operationId cannot be {request.OperationId}" }
                    }
                };
            }

            if (request.TargetId.Count() < 1 && !request.DeferredExecution)
            {
                return new GoForApprovalRespObj
                {
                    EnableWorkflow = true,
                    HasWorkflowAccess = true,
                    ApprovalProcessStarted = false,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage { FriendlyMessage = $"Invalid Call! targetId cannot be {request.TargetId}" }
                    }
                };
            }

            if (request.CompanyId <= 0)
            {
                return new GoForApprovalRespObj
                {
                    EnableWorkflow = true,
                    HasWorkflowAccess = true,
                    ApprovalProcessStarted = false,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage { FriendlyMessage = $"Invalid Call! companyId cannot be {request.CompanyId}" }
                    }
                };
            }
            if (request.StatusId < 0)
            {
                return new GoForApprovalRespObj
                {
                    EnableWorkflow = true,
                    HasWorkflowAccess = true,
                    ApprovalProcessStarted = false,
                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage { FriendlyMessage = $"Invalid Call! statusId cannot be {request.StatusId}" }
                    }
                };
            }
            return new GoForApprovalRespObj { EnableWorkflow = true, HasWorkflowAccess = true, Status = new APIResponseStatus { IsSuccessful = true, } };
        }

       
        private async Task SendMail(string email, string username, string comment)
        {
            var sm = new SendEmailCommand();
            sm.Subject = $"{comment} awaiting approval";
            sm.Content = $"Hello {username} <br/>" +
                $"You have a pending task on {comment} as the first approver";
            sm.ToAddresses.Add(new EmailAddressCommand { Address = email, Name = username });
            sm.SendIt = true;
            sm.SaveIt = true;
            var mailSent = await _email.BuildAndSaveEmail(sm);
            
            EmailMessage em = new EmailMessage
            {
                Subject = sm.Subject,
                Content = sm.Content,
                FromAddresses = mailSent.FromAddresses,
                ToAddresses = _mapper.Map<List<EmailAddress>>(sm.ToAddresses),
                SendIt = true,
        };
            em.Module = (int)Modules.CENTRAL;
            await _email.Send(em);
        }

    }
}
