using APIGateway.Contracts.Response.Workflow;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Queries.Workflow
{
    public class GetAllWorkflowLevelStaffQuery : IRequest<WorkflowlevelStafRespObj> { }

    public class GetWorkflowLevelStaffQuery : IRequest<WorkflowlevelStafRespObj> {
        public GetWorkflowLevelStaffQuery() { }
        public int WorkflowLevelStaffId { get; set; }
        public GetWorkflowLevelStaffQuery(int workflowLevelStaffId)
        {
            WorkflowLevelStaffId = workflowLevelStaffId;
        }
    }
    
    public class GetWorkflowLevelStaffsByStaffQuery : IRequest<WorkflowlevelStafRespObj>
    {
        public GetWorkflowLevelStaffsByStaffQuery() { }
        public int StaffId { get; set; }
        public GetWorkflowLevelStaffsByStaffQuery(int staffId)
        {
            StaffId = staffId;
        }
    }
    
    public class GetWorkflowLevelStaffsByWorkflowLevelQuery : IRequest<WorkflowlevelStafRespObj>
    {
        public GetWorkflowLevelStaffsByWorkflowLevelQuery() { }
        public int WorkflowLevelId { get; set; }
        public GetWorkflowLevelStaffsByWorkflowLevelQuery(int workflowLevelId)
        {
            WorkflowLevelId = workflowLevelId;
        }
    }


}
