using APIGateway.ActivityRequirement; 
using APIGateway.Contracts.Commands.Workflow;
using APIGateway.Contracts.Queries.Workflow; 
using APIGateway.Handlers.Common.Uploads_Downloads;
using APIGateway.Handlers.Permissions;
using APIGateway.Handlers.Workflow;
using GODPAPIs.Contracts.Commands.Workflow;
using GODPAPIs.Contracts.GeneralExtension;
using GODPAPIs.Contracts.RequestResponse.Workflow;
using GODPAPIs.Contracts.V1;
using GOSLibraries.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 
using System.Threading.Tasks;
using static APIGateway.Contracts.Queries.Workflow.GetWorkflowByOperationQuery;

namespace APIGateway.Controllers.V1
{ 
    [ERPAuthorize]
    public class WorkflowController : Controller
    {
        private readonly IMediator _mediator;
        public WorkflowController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
         
        [HttpPost(ApiRoutes.WorkdlowEndpoints.GO_FOR_APPROVAL)]
        public async Task<ActionResult<GoForApprovalRespObj>> GO_FOR_APPROVAL([FromBody] GoForApprovalCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet(ApiRoutes.WorkdlowEndpoints.GET_ALL_STAFF_AWAITING_APPROVALS)]
        public async Task<ActionResult<GoForApprovalRespObj>> GET_ALL_STAFF_AWAITING_APPROVALS()
        {
            var query = new GetAnApproverWorkflowTaskQuery();
            var response = await _mediator.Send(query); 
            return Ok(response); 
        }
        
        [HttpPost(ApiRoutes.WorkdlowEndpoints.STAFF_APPROVAL_REQUEST)]
        public async Task<IActionResult> APPROVAL_REQUEST([FromBody] StaffApprovalCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [ERPActivity(Action = UserActions.View, Activity = 18)]
        [HttpGet(ApiRoutes.WorkdlowEndpoints.GET_WORKFLOW_DETAILS)]
        public async Task<ActionResult<GoForApprovalRespObj>> GET_WORKFLOW_DETAILS([FromQuery] GetWorkflowdetailQuery query)
        { 
                return Ok(await _mediator.Send(query)); 
        }

        [ERPActivity(Action = UserActions.Add, Activity = 14)]
        [HttpPost(ApiRoutes.WorkdlowEndpoints.ADD_UPDATE_WKF_GROUP)]
        public async Task<IActionResult> UpdateWKF([FromBody] AddUpdateWorkflowGroupCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [ERPActivity(Action = UserActions.Delete, Activity = 14)]
        [HttpPost(ApiRoutes.WorkdlowEndpoints.DELETE_WKF_GROUP)]
        public async Task<IActionResult> DELETE_WKF_GROUP([FromBody] DeleteWorkflowGroupCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [ERPActivity(Action = UserActions.Delete, Activity = 14)]
        [HttpGet(ApiRoutes.WorkdlowEndpoints.GET_WKF_GROUP)]
        public async Task<IActionResult> GetWKF([FromQuery]GetWorkflowGroupQuery query)
        {
            var res = await _mediator.Send(query);
            return Ok(res);
        }

        [HttpGet(ApiRoutes.WorkdlowEndpoints.GET_ALL_WKF_GROUP)]
        public async Task<IActionResult> GetWKFGroups()
        {
            var query = new GetAllWorkflowGroupQuery();
            return Ok(await _mediator.Send(query));
        }

        
        [HttpGet(ApiRoutes.WorkdlowEndpoints.GET_ALL_OPERATION_TYPES)]
        public async Task<IActionResult> GetALLOperationTypes()
        {
            var query = new GetAllOperationTypesQuery();
            return Ok(await _mediator.Send(query)); 
        }

        [HttpGet(ApiRoutes.WorkdlowEndpoints.GET_ALL_OPERATIONS)]
        public async Task<IActionResult> GetALLOperations()
        {
            var query = new GetAllOperationQuery();
            return Ok(await _mediator.Send(query));
        }

        [ERPActivity(Action = UserActions.View, Activity = 14)]
        [HttpGet(ApiRoutes.WorkdlowEndpoints.GENERATE_EXCEL_WKF_group)]
        public async Task<IActionResult> GenerateExcelAsyncWkgroup()
        {
            var command = new GenerateExportWorkflowGroupCommand();
            return  Ok(await _mediator.Send(command));
        }

        [ERPActivity(Action = UserActions.View, Activity = 15)]
        [HttpGet(ApiRoutes.WorkdlowEndpoints.GENERATE_EXCEL_WKF_Level)]
        public async Task<IActionResult> GenerateExcelAsyncWklevel()
        {
            var command = new GenerateExportWorkflowLevelCommand();
            return Ok(await _mediator.Send(command));
        }

        [ERPActivity(Action = UserActions.Add, Activity = 14)]
        [HttpPost(ApiRoutes.WorkdlowEndpoints.UPLOAD_WORK_FLOW_GRP)]
        public async Task<IActionResult> UPLOAD_WORK_FLOW_GRP()
        {
            var command = new UploadWorkflowGroupCommand();
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        } 


        [HttpGet(ApiRoutes.WorkdlowEndpoints.GET_ALL_WKF_LEVEL)]
        public async Task<IActionResult> GetAllWorkflowLevel()
        {
            var query = new GetAllWorkflowLevelQuery();
            return Ok(await _mediator.Send(query));
        }

        [ERPActivity(Action = UserActions.View, Activity = 15)]
        [HttpGet(ApiRoutes.WorkdlowEndpoints.GET_WKF_LEVEL)]
        public async Task<IActionResult> GetWorkflowLevel([FromQuery]GetWorkflowLevelByIdQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet(ApiRoutes.WorkdlowEndpoints.GET_ALL_WKFL_BY_WKF_GROUP)]
        public async Task<IActionResult> GetAllWorkflowLevelBygroup([FromQuery]GetWorkflowLevelsByWorkflowGroupQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [ERPActivity(Action = UserActions.Add, Activity = 15)]
        [HttpPost(ApiRoutes.WorkdlowEndpoints.ADD_UPDATE_WKF_LEVEL)]
        public async Task<IActionResult> AddUpdateWKFLevel([FromBody]AddUpdateWorkflowLevelCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost(ApiRoutes.WorkdlowEndpoints.DELETE_WKF_LEVEL)]
        public async Task<IActionResult> DeleteWKFLevel([FromBody]DeleteWorkflowLevelCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [ERPActivity(Action = UserActions.Add, Activity = 15)]
        [HttpPost(ApiRoutes.WorkdlowEndpoints.UPLOAD_WKF_LEVEL)]
        public async Task<IActionResult> UploadWKFLevel(FileUploadObj objectFile)
        { 
            var command = new UploadWorkflowLevelCommand();
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [ERPActivity(Action = UserActions.Add, Activity = 17)]
        [HttpPost(ApiRoutes.WorkdlowEndpoints.ADD_UPDATE_WKF_LEVEL_STAFF)]
        public async Task<IActionResult> AddUpdateWKFLevelStaff([FromBody]AddUpdateWorkflowLevelStaffCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [ERPActivity(Action = UserActions.Delete, Activity = 17)]
        [HttpPost(ApiRoutes.WorkdlowEndpoints.DELETE_WKF_LEVEL_STAFF)]
        public async Task<IActionResult> DeleteWKFLevelStaff([FromBody]DeleteWorkflowLevelStaffCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet(ApiRoutes.WorkdlowEndpoints.GET_ALL_WKF_LEVEL_STAFF)]
        public async Task<IActionResult> GetAllWorkflowLevelByStaff()
        {
            var query = new GetAllWorkflowLevelStaffQuery();
            return Ok(await _mediator.Send(query));
        }

        [ERPActivity(Action = UserActions.View, Activity = 17)]
        [HttpGet(ApiRoutes.WorkdlowEndpoints.GET_WKF_LEVEL_STAFF)]
        public async Task<IActionResult> GetWorkflowLevelStaff([FromQuery]GetWorkflowLevelStaffQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet(ApiRoutes.WorkdlowEndpoints.GET_WKF_LEVEL_STAFF_BY_STAFFID)]
        public async Task<IActionResult> GetWorkflowLevelStaff([FromQuery]GetWorkflowLevelStaffsByStaffQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet(ApiRoutes.WorkdlowEndpoints.GET_WORKFLOW_BY_OPERATION)] 
        public async Task<IActionResult> GetWorkflowLevelStaff([FromQuery]GetWorkflowByOperationQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
        [ERPActivity(Action = UserActions.View, Activity = 18)]
        [HttpGet(ApiRoutes.WorkdlowEndpoints.GET_WORKFLOW)]
        public async Task<IActionResult> GetWorkflowLevelStaff([FromQuery]GetSingleWorkflowQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
        [ERPActivity(Action = UserActions.Add, Activity = 18)]
        [HttpPost(ApiRoutes.WorkdlowEndpoints.ADD_UPDATE_WORKFLOW)]
        public async Task<IActionResult> DeleteWKFLevelStaff([FromBody]AddUpdateWorkflowCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [ERPActivity(Action = UserActions.Add, Activity = 18)]
        [HttpPost(ApiRoutes.WorkdlowEndpoints.UPDATE_WKF_OPERATION)]
        public async Task<IActionResult> UPDATE_WKF_OPERATION([FromBody]UpdateWorkflowOperationCommand command)
        {
            if (command.WkflowOperations == null) return null;
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [ERPActivity(Action = UserActions.Delete, Activity = 18)]
        [HttpPost(ApiRoutes.WorkdlowEndpoints.DELETE_WORKFLOW)]
        public async Task<IActionResult> DELETE_WORKFLOW([FromBody]DeleteWorkflowCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
         
        [HttpPost(ApiRoutes.WorkdlowEndpoints.GET_ALL_WKF_OPERATION)]
        public async Task<IActionResult> GetAllworkflowOperation()
        {
            var query = new GetAllWorkflowOperationQuery();
            return Ok(await _mediator.Send(query));
        }

        [HttpGet(ApiRoutes.PersmissionsEndpoint.CAN_EDIT)]
        public async Task<IActionResult> CAN_EDIT()
        {
            return Ok(await _mediator.Send(new CanModifyQuery()));
        }

        [ERPActivity(Action = UserActions.View, Activity = 17)]
        [HttpGet(ApiRoutes.WorkdlowEndpoints.DOWNLOAD_LEVEL_STAFF)]
        public async Task<IActionResult> DOWNLOAD_LEVEL_STAFF()
        {
            return Ok(await _mediator.Send(new DownloadWorkflowStaffQuery()));
        }

        [ERPActivity(Action = UserActions.Add, Activity = 17)]
        [HttpPost(ApiRoutes.WorkdlowEndpoints.UPLOAD_LEVEL_STAFF)]
        public async Task<IActionResult> UPLOAD_LEVEL_STAFF()
        {
            var command = new UploadWorkflowStaffCommand();
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
    }
} 
