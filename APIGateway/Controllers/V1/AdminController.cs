using APIGateway.ActivityRequirement;
using APIGateway.Contracts.Commands.Admin;
using APIGateway.Contracts.Queries.Admin;
using APIGateway.Handlers.Admin;
using GODPAPIs.Contracts.Commands.Admin; 
using GODPAPIs.Contracts.V1; 
using GOSLibraries.Enums;
using MediatR; 
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc; 
using Puchase_and_payables.Handlers.Supplier.Settup; 
using System.Threading.Tasks;

namespace GODP.APIsContinuation.Controllers.V1
{
    [ERPAuthorize] 
    public class AdminController : Controller
    {
        private readonly IMediator _mediator; 
        public AdminController(
            IMediator mediator )
            { 
                _mediator = mediator; 
            }
         
        [HttpGet(ApiRoutes.AdminEndpoints.GET_ALL_ACTIVITY_PAREANTS)]
        public async Task<IActionResult> GetAllActivityParent()
        {
            var query = new GetAllActivityParentQuery();
            return Ok(await _mediator.Send(query)); 
        }

        [AllowAnonymous]
        [ERPActivity(Action = UserActions.Add, Activity = 3)]
        [HttpPost(ApiRoutes.AdminEndpoints.UPDATE_STAFF_USER)]
        public async Task<IActionResult> UpdateStaff([FromBody] UpdateStaffCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [AllowAnonymous] 
        [HttpGet(ApiRoutes.AdminEndpoints.GET_ALL_STAFF)]
        public async Task<IActionResult> GetAllStaff()
        { 
            var query = new GetAllStaffQuery();
            var response = await _mediator.Send(query); 
            return Ok(response); 
        }
        [ERPActivity(Action = UserActions.View, Activity = 3)]
        [HttpGet(ApiRoutes.AdminEndpoints.GET_STAFF)]
        public async Task<IActionResult> GetAllStaff([FromQuery] GetStaffQuery query)
        {
           return Ok(await _mediator.Send(query));
        }

        [ERPActivity(Action = UserActions.Add, Activity = 2)]
        [HttpPost(ApiRoutes.AdminEndpoints.UPDATE_ROLE_ACTIVITY)]
        public async Task<IActionResult> UpdateRoleActivity([FromBody] AddUpdateUserRoleActivityCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [ERPActivity(Action = UserActions.Delete, Activity = 2)]
        [HttpPost(ApiRoutes.AdminEndpoints.DELETE_ROLE)]
        public async Task<IActionResult> DELETE_ROLE([FromBody]DeleteRoleCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet(ApiRoutes.AdminEndpoints.GET_ALL_ROLES)]
        public async Task<IActionResult> GET_ALL_ROLES()
        {
            var query = new GetAllRolesQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }
        [ERPActivity(Action = UserActions.Delete, Activity = 3)]
        [HttpPost(ApiRoutes.AdminEndpoints.DELETE_STAFF)]
        public async Task<IActionResult> DELETE_STAFF([FromBody] DeleteStaffCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [ERPActivity(Action = UserActions.View, Activity = 2)]
        [HttpGet(ApiRoutes.AdminEndpoints.GET_ALL_ACTIVITIES_BY_ROLE_ID)]
        public async Task<IActionResult> GetAllActivitiesByRoleId([FromQuery] GetActivitiesByRoleIdQuery query)
        { 
            var response = await _mediator.Send(query);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [ERPActivity(Action = UserActions.View, Activity = 3)]
        [HttpGet(ApiRoutes.AdminEndpoints.GENERATE_STAFF_EXCEL)]
        public async Task<IActionResult> GENERATE_STAFF_EXCEL()
        {
            var command = new GenerateExportStaffCommad();
            return Ok(await _mediator.Send(command)); 
        }

        [ERPActivity(Action = UserActions.Add, Activity = 3)]
        [HttpPost(ApiRoutes.AdminEndpoints.UPLOAD_STAFF_EXCEL)]
        public async Task<IActionResult> UPLOAD_STAFF_EXCEL()
        {
            var command = new UploadStaffCommand();
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        
        [HttpPost(ApiRoutes.AdminEndpoints.ADD_MODULE)]
        public async Task<IActionResult> ADD_MODULE([FromBody] AddUpdateSolutionModuleCommand command)
        { 
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }


        [HttpGet(ApiRoutes.AdminEndpoints.GET_ALL_MODULE)]
        public async Task<IActionResult> GET_ALL_MODULE()
        {
            var query = new GetAllSolutionModuleQuery();
            return Ok(await _mediator.Send(query));
        }

        [HttpGet(ApiRoutes.AdminEndpoints.GET_SINGLE_MODULE)]
        public async Task<IActionResult> GET_SINGLE_MODULE([FromQuery] GetSingleSolutionModuleQueryCommand query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpPost(ApiRoutes.AdminEndpoints.DELETE_MODULE)]
        public async Task<IActionResult> GET_SINGLE_MODULE([FromQuery] DeleteSolutionModuleCommand command)
        { 
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        
        [HttpGet(ApiRoutes.AdminEndpoints.GET_ALL_ACTIVITIES)]
        public async Task<IActionResult> GET_ALL_ACTIVITIES()
        {
            var query = new GetAllActivityQuery();
            return Ok(await _mediator.Send(query));
        }


        [AllowAnonymous]
        [HttpGet(ApiRoutes.AdminEndpoints.GET_ALL_QUESTIONS)]
        public async Task<IActionResult> GET_ALL_QUESTIONS()
        {
            var query = new GetAllQuestionsQuery();
            return Ok(await _mediator.Send(query));
        }

        [ERPActivity(Action = UserActions.Delete, Activity = 7)]
        [HttpPost(ApiRoutes.AdminEndpoints.DELETE_QUESTION)]
        public async Task<IActionResult> DELETE_QUESTION([FromQuery] DeleteQuestionsCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [ERPActivity(Action = UserActions.View, Activity = 7)]
        [HttpGet(ApiRoutes.AdminEndpoints.GET_QUESTION)]
        public async Task<IActionResult> GET_QUESTION([FromQuery] GetQuestionQueryQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [ERPActivity(Action = UserActions.Add, Activity = 7)]
        [HttpPost(ApiRoutes.AdminEndpoints.ADD_UPDATE_QUESTION)]
        public async Task<IActionResult> ADD_UPDATE_QUESTION([FromBody] AddUpdateQuestionsCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
         
        [HttpGet(ApiRoutes.AdminEndpoints.GET_THIS_USER_ROLES)]
        public async Task<IActionResult> GET_ALL_USER_ROLES()
        {
            var query = new GetUserRolesQuery();
            return Ok(await _mediator.Send(query));
        }


        [ERPActivity(Action = UserActions.Add, Activity = 3)]
        [HttpPost(ApiRoutes.AdminEndpoints.RESET_PROFILE)]
        public async Task<IActionResult> RESET_PROFILE([FromBody] ResetStaffProfileCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        
    }
}
