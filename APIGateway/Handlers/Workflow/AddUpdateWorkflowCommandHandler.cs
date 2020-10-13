using APIGateway.Contracts.Commands.Workflow;
using APIGateway.Data;
using APIGateway.Repository.Interface.Workflow;
using GODP.APIsContinuation.DomainObjects.UserAccount;
using GODP.APIsContinuation.DomainObjects.Workflow; 
using GOSLibraries.GOS_Error_logger.Service;
using GOSLibraries.GOS_API_Response;
using GODPAPIs.Contracts.RequestResponse.Workflow; 
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GOSLibraries.Enums;

namespace APIGateway.Handlers.Workflow
{
    public class AddUpdateWorkflowCommandHandler : IRequestHandler<AddUpdateWorkflowCommand, WorkflowRespObj>
    {
        private readonly ILoggerService _logger;
        private readonly IWorkflowRepository _repo;
        private readonly UserManager<cor_useraccount> _userManger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext; 
        public AddUpdateWorkflowCommandHandler(ILoggerService loggerService, IWorkflowRepository repository,
            UserManager<cor_useraccount> userManger, IHttpContextAccessor httpContextAccessor, DataContext dataContext)
        {
            _logger = loggerService;
            _repo = repository;
            _userManger = userManger;
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<WorkflowRespObj> Handle(AddUpdateWorkflowCommand request, CancellationToken cancellationToken)
        {
			try
			{
                if (request.WorkflowId > 0)
                {
                    _repo.DeleteWorkflowAsync(request.WorkflowId);
                }

                List<cor_workflowaccess> wkfAccesses = new List<cor_workflowaccess>();
                List<cor_workflowdetailsaccess> wkfDetailsAccesses = new List<cor_workflowdetailsaccess>();
                List<cor_workflowdetails> wkfDetails = new List<cor_workflowdetails>();
                cor_workflow workflow = new cor_workflow();

                var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(x => x.Type == "userId").Value;
                var user = await _userManger.FindByIdAsync(currentUserId);

                #region Check if workflow with selected access Ids exist for this company

                //if (await _repo.CheckIfOperationAndAccessExistAsync(request.OperationId, request.WorkflowAccessIds))
                //    return new WorkflowRespObj
                //    {
                //        Status = new APIResponseStatus
                //        {
                //            IsSuccessful = false,
                //            Message = new APIResponseMessage { FriendlyMessage = "Workflow with same operation and access level is already added" }
                //        }
                //    };

                #endregion
                using (var _trans = await _dataContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                         
                        #region Check if office access selected matches the companystructure from repository

                        if (request.Details != null)
                            {
                                foreach (var workflowDetail in request.Details)
                                {
                                    if (workflowDetail.AccessOfficeIds.Length > 0)
                                    {
                                        var result = (from compStruct in _dataContext.cor_companystructure
                                                      join compStructDef in _dataContext.cor_companystructuredefinition on compStruct.StructureTypeId equals compStructDef.StructureDefinitionId
                                                      where workflowDetail.AccessOfficeIds.Contains(compStruct.CompanyStructureId) || workflowDetail.AccessOfficeIds.Contains((int)compStruct.ParentCompanyID)
                                                      select compStruct.CompanyStructureId).ToArray();

                                        string OfficeAccessIds = string.Join(',', result);

                                        var singleDetail = new cor_workflowdetails
                                        {
                                            WorkflowId = request.WorkflowId,//to be looked into
                                            WorkflowGroupId = workflowDetail.WorkflowGroupId,
                                            WorkflowLevelId = workflowDetail.WorkflowLevelId,
                                            AccessId = workflowDetail.AccessId,
                                            Position = workflowDetail.Position,
                                            OfficeAccessIds = OfficeAccessIds,
                                            Active = false,
                                            Deleted = false,
                                            CreatedBy = user.UserName,
                                            CreatedOn = DateTime.Now,
                                            UpdatedBy = user.UserName,
                                            UpdatedOn = DateTime.Now,
                                            OperationId = request.OperationId
                                        };
                                        wkfDetails.Add(singleDetail);
                                    }

                                }
                            }
                        else
                            return new WorkflowRespObj
                            {
                                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "No Workflow details added for this settup"} }
                            };

                        #endregion

                        #region At this point, we check for the workflow access
                        if (request.WorkflowAccessIds.Length > 0)
                        {
                            var CselectedCompanyStructureIds = (from compStruct in _dataContext.cor_companystructure
                                                                join compStructdef in _dataContext.cor_companystructuredefinition on compStruct.StructureTypeId equals compStructdef.StructureDefinitionId
                                                                where request.WorkflowAccessIds.Contains(compStruct.CompanyStructureId) || request.WorkflowAccessIds.Contains((int)compStruct.ParentCompanyID)
                                                                select compStruct.CompanyStructureId).ToArray();
                            if (CselectedCompanyStructureIds.Length > 0)
                            {
                                foreach (var officeAccessId in CselectedCompanyStructureIds)
                                {
                                    var access = new cor_workflowaccess
                                    {
                                        OfficeAccessId = officeAccessId,
                                        OperationId = request.OperationId,
                                        Active = true,
                                        Deleted = false,
                                        CreatedBy = user.UserName,
                                        CreatedOn = DateTime.Now,
                                        UpdatedBy = user.UserName,
                                        UpdatedOn = DateTime.Now,
                                    };
                                    wkfAccesses.Add(access);
                                }
                            }
                        }
                        else
                            return new WorkflowRespObj
                            {
                                Status = new APIResponseStatus { IsSuccessful = false, Message = new APIResponseMessage { FriendlyMessage = "No Workflow Accesss added for this workflow settup" } }
                            };
                        #endregion

                        #region At this point, there is a check if exist on workflow (ADD||UPDATE)

                        bool output = false;
                        if (request.WorkflowId > 0)
                            {
                                workflow = _dataContext.cor_workflow.Find(request.WorkflowId);
                                if (workflow != null)
                                {
                                    var targetDetails = _dataContext.cor_workflowdetails.Where(x => x.WorkflowId == workflow.WorkflowId).ToList();
                                    var targetAccesses = _dataContext.cor_workflowaccess.Where(x => x.WorkflowId == workflow.WorkflowId).ToList();

                                    if (targetDetails.Any())
                                    {
                                        foreach (var item in targetDetails)
                                        {
                                            _dataContext.cor_workflowdetails.Remove(item);
                                        }
                                    }
                                    if (targetAccesses.Any())
                                    {
                                        foreach (var item in targetAccesses)
                                        {
                                            _dataContext.cor_workflowaccess.Remove(item);
                                        }
                                    }
                                    workflow.WorkflowName = request.WorkflowName;
                                    workflow.WorkflowAccessId = request.WorkflowAccessId;
                                    workflow.OperationId = request.OperationId;
                                    workflow.ApprovalStatusId = workflow.ApprovalStatusId;//(int)ApprovalStatus.Approved; (question)="why us it initialize with approved"
                                    workflow.cor_workflowaccess = wkfAccesses;
                                    workflow.cor_workflowdetails = wkfDetails;
                                    workflow.Active = true;
                                    workflow.Deleted = false;
                                    workflow.UpdatedBy = user.UserName;
                                    workflow.UpdatedOn = DateTime.Now;
                                    _dataContext.Entry(workflow).CurrentValues.SetValues(workflow);
                                }
                            }
                            else
                            {
                                workflow = new cor_workflow
                                {
                                    WorkflowName = request.WorkflowName,
                                    WorkflowAccessId = request.WorkflowAccessId,
                                    OperationId = request.OperationId,
                                    ApprovalStatusId = (int)ApprovalStatus.Approved, 
                                    Active = true,
                                    Deleted = false,
                                    CreatedBy = workflow.UpdatedBy = user.UserName,
                                    CreatedOn = DateTime.Now,
                                    UpdatedBy = workflow.UpdatedBy = user.UserName,
                                    UpdatedOn = DateTime.Now,
                                };
                                _dataContext.cor_workflow.Add(workflow);
                        
                            if (await _dataContext.SaveChangesAsync() > 0)
                            {
                                foreach(var item in wkfAccesses)
                                {
                                    item.WorkflowId = workflow.WorkflowId; 
                                    await _dataContext.cor_workflowaccess.AddAsync(item);
                                }
                                foreach (var item in wkfDetails)
                                {
                                    item.WorkflowId = workflow.WorkflowId;
                                    await _dataContext.cor_workflowdetails.AddAsync(item); 
                                } 
                                await _dataContext.SaveChangesAsync();
                            }
                        }
                        #endregion

                        await _repo.AddWorkFlowDetailsAccessAsync();
                        await _trans.CommitAsync();
                        return new WorkflowRespObj
                        {
                            Status = new APIResponseStatus
                            {
                                IsSuccessful = true,
                                Message =new APIResponseMessage
                                {
                                    FriendlyMessage = "Successful"
                                }
                            }
                        };
                    }
                    catch (Exception ex)
                    {
                       await _trans.RollbackAsync();
                        throw new Exception(ex.Message);
                    }
                    finally { await _trans.DisposeAsync(); }
                }
            }
			catch (Exception ex)
			{
                #region Log error to file 
                var errorCode = ErrorID.Generate(4);
                _logger.Error($"ErrorID : AddUpdateWorkflowCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}");
                return new WorkflowRespObj
                {

                    Status = new APIResponseStatus
                    {
                        IsSuccessful = false,
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = "Error occured!! Unable to process item",
                            MessageId = errorCode,
                            TechnicalMessage = $"ErrorID : AddUpdateWorkflowCommandHandler{errorCode} Ex : {ex?.Message ?? ex?.InnerException?.Message} ErrorStack : {ex?.StackTrace}"
                        }
                    }
                };
                #endregion
            }
        }
    }
}
