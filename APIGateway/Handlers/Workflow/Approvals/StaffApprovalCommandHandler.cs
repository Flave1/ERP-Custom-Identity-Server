using APIGateway.Contracts.Commands.Email;
using APIGateway.Contracts.Commands.Workflow;
using APIGateway.Data;
using APIGateway.DomainObjects.Workflow;
using APIGateway.MailHandler;
using APIGateway.MailHandler.Service;
using APIGateway.Repository.Interface.Workflow;
using AutoMapper;
using GODP.APIsContinuation.DomainObjects.UserAccount; 
using GODPAPIs.Contracts.RequestResponse.Workflow;
using GOSLibraries.Enums;
using GOSLibraries.GOS_API_Response;
using GOSLibraries.GOS_Error_logger.Service;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Workflow.Approvals
{
    public class StaffApprovalCommandHandler : IRequestHandler<StaffApprovalCommand, StaffApprovalRegRespObj>
    {
        private readonly IWorkflowRepository _repo;
        private readonly ILoggerService _logger;
        private readonly UserManager<cor_useraccount> _userManager;
        private readonly IHttpContextAccessor _accessor;
        private readonly RoleManager<cor_userrole> _roleManager;
        private readonly DataContext _data;
        private List<cor_workflowtask> _CurrentPositionTasks = new List<cor_workflowtask>();
        private List<cor_workflowtask> _AllTaskForThisTarget = new List<cor_workflowtask>();
        private cor_workflowtask _CurrentStaffTask = new cor_workflowtask();
        private readonly IMediator _mediator;
        private readonly IEmailService _email;
        private readonly IMapper _mapper; 
        public StaffApprovalCommandHandler(
            DataContext data,
            IMediator mediator,
            IWorkflowRepository workflowRepository, 
            ILoggerService loggerService, 
            RoleManager<cor_userrole> roleManager,
            IMapper mapper,
            UserManager<cor_useraccount> userManager, 
            IHttpContextAccessor httpContextAccessor,
            IEmailService email)
        {
            _repo = workflowRepository;
            _userManager = userManager;
            _logger = loggerService;
            _accessor = httpContextAccessor;
            _roleManager = roleManager;
            _data = data;
            _email = email;
            _mediator = mediator;
            _mapper = mapper;
        }
        public async Task<StaffApprovalRegRespObj> Handle(StaffApprovalCommand request, CancellationToken cancellationToken)
        {
            var _ApiResponse = new StaffApprovalRegRespObj { Status = new APIResponseStatus { IsSuccessful = true, Message = new APIResponseMessage() } };
            try
            {
                if (request.ApprovalStatus == (int)ApprovalStatus.Revert && request.ReferredStaffId < 1)
                {
                    _ApiResponse.Status.IsSuccessful = false;
                    _ApiResponse.Status.Message.FriendlyMessage = "Please select staff to revert to";
                    return _ApiResponse;
                }

                var currentUserId = _accessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;
                var user = await _userManager.FindByIdAsync(currentUserId);

                var taskList = await _repo.GetAllWorkflowTaskAsync();
                _AllTaskForThisTarget = taskList.Where(d => d.TargetId == request.TargetId && d.WorkflowToken.Trim().ToLower() == request.WorkflowToken.Trim().ToLower()).ToList();
                _CurrentStaffTask = taskList.LastOrDefault(d => d.TargetId == request.TargetId && d.StaffEmail.Trim().ToLower() == user.Email.Trim().ToLower());

                if (_CurrentStaffTask == null)
                {
                    _ApiResponse.Status.IsSuccessful = false;
                    _ApiResponse.Status.Message.FriendlyMessage = "Task not available";
                    return _ApiResponse;
                }
                using (var _trans = await _data.Database.BeginTransactionAsync())
                {
                    try
                    {
                        if (request.ApprovalStatus == (int)ApprovalStatus.Approved)
                        {
                            var allTaskForThisTarget = await _repo.GetCurrentWorkTaskAsync(_CurrentStaffTask.OperationId, 
                                request.TargetId, _CurrentStaffTask.WorkflowId, request.WorkflowToken);
                            var pendinTask = allTaskForThisTarget.OrderBy(g => g.Position).FirstOrDefault(x => x.WorkflowTaskStatus == (int)WorkflowTaskStatus.Created);

                            if (await UpdateApprovedTasksWithMuthipleStaffAsync(request))
                            {
                                if (await SendToNextStaffAsync(request))
                                {
                                    await _trans.CommitAsync();
                                    _ApiResponse.ResponseId = (int)ApprovalStatus.Processing; 
                                    _ApiResponse.Status.Message.FriendlyMessage = "Successfully approved";
                                    if (await IsFinalApprovalAsync(request))
                                    {
                                         await FinalizeAsync(request);
                                        _ApiResponse.ResponseId = (int)ApprovalStatus.Approved; 
                                        _ApiResponse.Status.Message.FriendlyMessage = "Final Aproval Successful";
                                    }
                                    return _ApiResponse;
                                }
                            }
                            else
                            {
                                _CurrentStaffTask.WorkflowTaskStatus = (int)WorkflowTaskStatus.Done;
                                _CurrentStaffTask.TargetId = request.TargetId;
                                await _repo.CreateUpdateWorkflowTaskAsync(_CurrentStaffTask);
                                if (await SendToNextStaffAsync(request))
                                {
                                    await _trans.CommitAsync();
                                    _ApiResponse.ResponseId = (int)ApprovalStatus.Processing; 
                                    _ApiResponse.Status.Message.FriendlyMessage = "Successfully approved";
                                    if (await IsFinalApprovalAsync(request))
                                    {
                                         await FinalizeAsync(request);
                                        _ApiResponse.ResponseId = (int)ApprovalStatus.Approved; 
                                        _ApiResponse.Status.Message.FriendlyMessage = "Final Aproval Successful";
                                    }
                                    return _ApiResponse;
                                }
                                if (await IsFinalApprovalAsync(request))
                                {
                                    await FinalizeAsync(request);
                                    await _trans.CommitAsync();
                                    _ApiResponse.ResponseId = (int)ApprovalStatus.Approved; 
                                    _ApiResponse.Status.Message.FriendlyMessage = "Final Aproval Successful";
                                    return _ApiResponse;
                                }
                            }

                        }
                        if (request.ApprovalStatus == (int)ApprovalStatus.Disapproved)
                        {  
                            if(await DisapproveAllTaskForthisOperationAsync())
                            {
                                await _trans.CommitAsync();
                                _ApiResponse.ResponseId = (int)ApprovalStatus.Disapproved;
                                _ApiResponse.Status.Message.FriendlyMessage = "Successfully Disapproved";
                            }
                            return _ApiResponse;
                        }
                        if (request.ApprovalStatus == (int)ApprovalStatus.Revert)
                        {
                            if (!await StaffToBeRevertedToExistAsync(request))
                            {
                                await _trans.RollbackAsync();
                                _ApiResponse.Status.IsSuccessful = false;
                                _ApiResponse.Status.Message.FriendlyMessage = "Staff not found in approval line";
                                return _ApiResponse;
                            }
                            await UpdateSingleOrMultipleRevertingStaffAsync(request); 
                            if (await RevertToSingleOrMultipleStaffAsync(request))
                            {
                                await _trans.CommitAsync();
                                _ApiResponse.ResponseId = (int)ApprovalStatus.Revert;
                                _ApiResponse.Status.Message.FriendlyMessage = "Successfully Referred";
                                return _ApiResponse;
                            }
                        }
                    }
                    catch (SqlException ex) {await _trans.RollbackAsync();  }
                    finally { await _trans.DisposeAsync(); }
                }

                _ApiResponse.Status.IsSuccessful = false;
                _ApiResponse.Status.Message.FriendlyMessage = "No Approval Status selected";
                return _ApiResponse;
            }
            catch (Exception ex)
            {
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"); 
                _ApiResponse.Status.IsSuccessful = false;
                _ApiResponse.Status.Message.FriendlyMessage = "Error occured!! Unable to process request";
                _ApiResponse.Status.Message.TechnicalMessage = $"ErrorID : {errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}";
                _ApiResponse.Status.Message.MessageId = errorCode;
                return _ApiResponse; 
                #endregion
            }
        }

        private async Task<bool> StaffToBeRevertedToExistAsync(StaffApprovalCommand request)
        {
            var allTaskForThisTarget = await _repo.GetCurrentWorkTaskAsync(_CurrentStaffTask.OperationId, request.TargetId, _CurrentStaffTask.WorkflowId, request.WorkflowToken);
            return allTaskForThisTarget.Any(x => x.StaffId == request.ReferredStaffId);
        }
        private async Task<bool> UpdateSingleOrMultipleRevertingStaffAsync(StaffApprovalCommand request)
        {
            var allTaskForThisTarget = await _repo.GetCurrentWorkTaskAsync(_CurrentStaffTask.OperationId, request.TargetId, _CurrentStaffTask.WorkflowId, request.WorkflowToken);
            if (_CurrentStaffTask != null)
            {
                _CurrentPositionTasks = allTaskForThisTarget.Where(s => s.Position == _CurrentStaffTask.Position).ToList();
            }
            if (_CurrentPositionTasks.Count() > 1)
            {
                foreach (var item in _CurrentPositionTasks)
                {
                    item.ApprovalStatus = (int)ApprovalStatus.Revert;
                    item.WorkflowTaskStatus = (int)WorkflowTaskStatus.Created;
                    item.TargetId = request.TargetId;
                    item.IsMailSent = false;
                    await _repo.CreateUpdateWorkflowTaskAsync(_CurrentStaffTask);
                    //Send Email here notification to the staffs
                }
                return true;
            }
            else
            {
                _CurrentStaffTask.ApprovalStatus = (int)ApprovalStatus.Revert;
                _CurrentStaffTask.WorkflowTaskStatus = (int)WorkflowTaskStatus.Created; 
                _CurrentStaffTask.TargetId = request.TargetId;
                _CurrentStaffTask.IsMailSent = false;
                await _repo.CreateUpdateWorkflowTaskAsync(_CurrentStaffTask);
                //Send Email here notification to the staff
                return true;
            }
        }
        private async Task<bool> RevertToSingleOrMultipleStaffAsync(StaffApprovalCommand request)
        {
            var allTaskForThisTarget = await _repo.GetCurrentWorkTaskAsync(_CurrentStaffTask.OperationId, request.TargetId, _CurrentStaffTask.WorkflowId, request.WorkflowToken);
            var approvedTask = allTaskForThisTarget.LastOrDefault(x => x.StaffId == request.ReferredStaffId);
            if (approvedTask != null)
            {
                _CurrentPositionTasks = allTaskForThisTarget.Where(s => s.Position == approvedTask.Position).ToList();
            }
            if (_CurrentPositionTasks.Count() > 1)
            {
                foreach (var item in _CurrentPositionTasks)
                {
                    item.IsMailSent = true;
                    item.ApprovalStatus = (int)ApprovalStatus.Revert;
                    item.WorkflowTaskStatus = (int)WorkflowTaskStatus.Created;
                    await _repo.CreateUpdateWorkflowTaskAsync(item);
                    //Send Email here notification to the reverted staff
                }
                return true;
            }
            else
            {
                approvedTask.IsMailSent = true;
                approvedTask.ApprovalStatus = (int)ApprovalStatus.Revert;
                approvedTask.WorkflowTaskStatus = (int)WorkflowTaskStatus.Created;
                await _repo.CreateUpdateWorkflowTaskAsync(approvedTask);
                return true;
            } 
        }
        private async Task<bool> DisapproveAllTaskForthisOperationAsync()
        {
            if(_AllTaskForThisTarget.Count() > 0)
            {
                foreach (var currentTask in _AllTaskForThisTarget)
                { 
                    currentTask.ApprovalStatus = (int)ApprovalStatus.Disapproved;
                    currentTask.WorkflowTaskStatus = (int)WorkflowTaskStatus.Disapproved;
                    currentTask.IsMailSent = true;
                    await _repo.CreateUpdateWorkflowTaskAsync(currentTask);
                }
                return true;
            }
            return false;
        }
        private async Task<bool> UpdateApprovedTasksWithMuthipleStaffAsync(StaffApprovalCommand request)
        {
            _CurrentPositionTasks = _AllTaskForThisTarget.Where(s => s.Position == _CurrentStaffTask.Position).ToList();
            if (_CurrentPositionTasks.Count() > 1)
            {
                foreach (var item in _CurrentPositionTasks)
                {
                    item.WorkflowTaskStatus = (int)WorkflowTaskStatus.Done;
                    item.TargetId = request.TargetId;
                    await _repo.CreateUpdateWorkflowTaskAsync(item);
                    //Send Email on the next available task to be approved
                }
                return true;
            }
            return false;
        }

        private async Task<bool> FinalizeAsync(StaffApprovalCommand request)
        {
            var allTaskForThisTarget = await _repo.GetCurrentWorkTaskAsync(_CurrentStaffTask.OperationId, request.TargetId, _CurrentStaffTask.WorkflowId, request.WorkflowToken);
            if (allTaskForThisTarget.Count() > 0) 
            {
                foreach (var item in allTaskForThisTarget)
                {
                    item.ApprovalStatus = (int)ApprovalStatus.Approved;
                    item.WorkflowTaskStatus = (int)WorkflowTaskStatus.Completed; 
                    await _repo.CreateUpdateWorkflowTaskAsync(item);
                }
                await SendFinalApprovalMail(allTaskForThisTarget.First());
                return true;
            }
            return false; 
        }

        private async Task<bool> IsFinalApprovalAsync(StaffApprovalCommand request)
        {
            var items =  await _repo.GetCurrentWorkTaskAsync(_CurrentStaffTask.OperationId, request.TargetId, _CurrentStaffTask.WorkflowId, request.WorkflowToken);
            var res =  items.All(d => d.WorkflowTaskStatus == (int)WorkflowTaskStatus.Done);
            return res;
        }

        private async Task<bool> UpdatedNextTaskWithMultipleStaffAsync(List<cor_workflowtask> currentPositionTasks)
        {
            
            if (currentPositionTasks.Count() > 1)
            {
                foreach (var item in currentPositionTasks)
                {
                    item.IsMailSent = true;
                    await SendInProcessApprovalMail(item.StaffEmail, null, item.Comment);
                    await _repo.CreateUpdateWorkflowTaskAsync(item);
                    //Send Email on the next available task to be approved
                }
                return true; 
            }
            return false;
        }

        private async Task<bool> SendToNextStaffAsync(StaffApprovalCommand request)
        {
            var allTaskForThisTarget = await _repo.GetCurrentWorkTaskAsync(_CurrentStaffTask.OperationId, request.TargetId, _CurrentStaffTask.WorkflowId, request.WorkflowToken);
            var newPendingTask = allTaskForThisTarget.OrderBy(g => g.Position).FirstOrDefault(x => x.WorkflowTaskStatus == (int)WorkflowTaskStatus.Created);
            if(newPendingTask != null)
            {
                _CurrentPositionTasks = allTaskForThisTarget.Where(d => d.Position == newPendingTask.Position).ToList();
            }
            if (!await UpdatedNextTaskWithMultipleStaffAsync(_CurrentPositionTasks))
            {
                if (newPendingTask != null)
                {
                    newPendingTask.IsMailSent = true;
                    await SendInProcessApprovalMail(newPendingTask.StaffEmail, null, newPendingTask.Comment);
                    await _repo.CreateUpdateWorkflowTaskAsync(newPendingTask);
                    return true;
                } 
            } 
            return true;
            //Send Email on the next available task to be approved
        }

        private async Task SendInProcessApprovalMail(string email, string username, string comment)
        {
            var name = email.Split('@')[0];
            var sm = new SendEmailCommand();
            sm.Subject = $"{comment} awaiting approval";

            sm.Content = $"Hello {name} <br/>" +
                $"You have a pending task on {comment} to be approved";

            sm.ToAddresses.Add(new EmailAddressCommand { Address = email, Name = name });
            sm.SaveIt = true;
            var mailSent = await _email.BuildAndSaveEmail(sm);

            EmailMessage em = new EmailMessage
            {
                Subject = sm.Subject,
                Content = sm.Content,
                FromAddresses = mailSent.FromAddresses,
                ToAddresses = _mapper.Map<List<EmailAddress>>(sm.ToAddresses),
            };
            em.SendIt = true;
          
            await _email.Send(em);
        }


        private async Task SendFinalApprovalMail(cor_workflowtask task) 
        {
            var initiator = await _userManager.FindByIdAsync(task.CreatedBy);
            if(initiator != null)
            {
                var sm = new SendEmailCommand();
                sm.Subject = $"{task.Comment} <b>Final approval process successful <b>";

                sm.Content = $"Hello {initiator.UserName} <br/>" +
                    $"This is to inform you of {task.Comment}, initiated by you on {task.CreatedOn} has been finally approved on {DateTime.Today}";

                sm.ToAddresses.Add(new EmailAddressCommand { Address = initiator.Email, Name = initiator.UserName });
                var mailSent = await _email.BuildAndSaveEmail(sm);

                EmailMessage em = new EmailMessage
                {
                    Subject = sm.Subject,
                    Content = sm.Content,
                    FromAddresses = mailSent.FromAddresses,
                    ToAddresses = _mapper.Map<List<EmailAddress>>(sm.ToAddresses),
                };
                await _email.Send(em);
            }
           
        }
    }
}
