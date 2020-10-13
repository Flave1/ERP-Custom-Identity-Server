using APIGateway.Contracts.Response.Workflow;
using GODPAPIs.Contracts.RequestResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Commands.Workflow
{
    public class AddUpdateWorkflowLevelCommand : IRequest<WorkflowLevelRegRespObj>
    {
        public int WorkflowLevelId { get; set; }

        public string WorkflowLevelName { get; set; }

        public int WorkflowGroupId { get; set; }

        public int Position { get; set; }

        public string RoleId { get; set; }

        public bool RequiredLimit { get; set; }

        public string LimitAmount { get; set; }

        public bool? CanModify { get; set; }

        public bool? Active { get; set; }

        public bool? Deleted { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }

    public class DeleteWorkflowLevelCommand : IRequest<DeleteRespObj>
    {
        public List<int> WorkflowLevelIds { get; set; }
    }

    public class UploadWorkflowLevelCommand : IRequest<WorkflowLevelRegRespObj>
    {
        public byte[] FileBytes { get; set; }
        public string CreatedBy { get; set; }
    }

    public class GenerateExportWorkflowLevelCommand : IRequest<byte[]> { }
}
