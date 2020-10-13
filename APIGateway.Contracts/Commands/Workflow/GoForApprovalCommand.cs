using GODPAPIs.Contracts.RequestResponse.Workflow;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Commands.Workflow
{
    public class GoForApprovalCommand : IRequest<GoForApprovalRespObj>
    {
        public int StaffId { get; set; }
        public int CompanyId { get; set; }
        public int StatusId { get; set; }
        public List<int> TargetId { get; set; }
        public string Comment { get; set; }
        public int OperationId { get; set; }
        public bool DeferredExecution { get; set; }
        public int WorkflowId { get; set; }
        public bool ExternalInitialization { get; set; }
        public bool EmailNotification { get; set; }
        public int WorkflowTaskId { get; set; }
        public int ApprovalStatus { get; set; }

    }

    public class StaffApprovalCommand : IRequest<StaffApprovalRegRespObj>
    {
        public int ApprovalStatus { get; set; }
        public string ApprovalComment { get; set; }
        public int TargetId { get; set; }
        public string WorkflowToken { get; set; }
        public int ReferredStaffId { get; set; }
    }
}
