using APIGateway.Contracts.Commands.Email;
using APIGateway.Handlers.Common;
using APIGateway.MailHandler.Send_Recieve_Handler;
using GODPAPIs.Contracts.V1;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Puchase_and_payables.Handlers.Supplier.Settup;
using System.Threading.Tasks; 

namespace APIGateway.Controllers.V1
{
    public class EmailController : Controller
    {
        private readonly IMediator _mediator;
        public EmailController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost(ApiRoutes.EmailEndpoint.SEND_EMAIL)]
        public async Task<IActionResult> SEND_EMAIL([FromBody] SendEmailCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        
        [HttpGet(ApiRoutes.EmailEndpoint.GET_USER_EMAILS)]
        public async Task<IActionResult> GET_USER_EMAILS([FromQuery] GetCurrentUserMailsQuery query)
        { 
            return Ok(await _mediator.Send(query)); 
        }
        [HttpGet(ApiRoutes.EmailEndpoint.GET_SINGLE_EMAIL)]
        public async Task<IActionResult> GET_SINGLE_EMAIL([FromQuery] GetSingleEmailDetailsComandQuery query)
        { 
            return Ok(await _mediator.Send(query));
        }

         

        [HttpPost(ApiRoutes.EmailEndpoint.ADD_UPDATE_EMAIL_CONFIG)]
        public async Task<IActionResult> ADD_UPDATE_EMAIL_CONFIG([FromBody] AddUpdateEmailConfigCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet(ApiRoutes.EmailEndpoint.GET_ALL_EMAIL_CONFIG)]
        public async Task<IActionResult> GET_ALL_EMAIL_CONFIG()
        {
            var query = new GetAllEmailConfigQuery();
            return Ok(await _mediator.Send(query));
        }
        [HttpGet(ApiRoutes.EmailEndpoint.GET_EMAIL_CONFIG)]
        public async Task<IActionResult> GET_EMAIL_CONFIG([FromQuery] GetEmailConfigQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
        [HttpPost(ApiRoutes.EmailEndpoint.DELETE_EMAIL_CONFIG)]
        public async Task<IActionResult> DELETE_EMAIL_CONFIG([FromBody] DeleteEmailConfigCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpPost(ApiRoutes.EmailEndpoint.SEND_EMAIL_TO_SPECIFIC_OFFICERS)]
        public async Task<IActionResult> SEND_EMAIL_TO_SPECIFIC_OFFICERS([FromBody] SendEmailToSpicificOfficersCommand command)
        {
            var response = await _mediator.Send(command);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        
    }
}
