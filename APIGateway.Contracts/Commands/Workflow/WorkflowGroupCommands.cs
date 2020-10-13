using GODPAPIs.Contracts.GeneralExtension;
using GODPAPIs.Contracts.RequestResponse;
using GODPAPIs.Contracts.RequestResponse.Workflow;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GODPAPIs.Contracts.Commands.Workflow
{
    public class AddUpdateWorkflowGroupCommand : IRequest<WorkflowGroupRegRespObj>
    {
        public int WorkflowGroupId { get; set; }
        public string WorkflowGroupName { get; set; }
        public bool? Active { get; set; }
        public bool? Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }

    public class DeleteWorkflowGroupCommand : IRequest<DeleteRespObj>
    {
        public List<int> WorkflowGroupIds { get; set; }
    }

     public class GenerateExportWorkflowGroupCommand : IRequest<byte[]> { }
   
    public class UploadWorkflowGroupCommand : IRequest<FileUploadRespObj> {
    public byte[] Filebyte { get; set; }
    }
}
