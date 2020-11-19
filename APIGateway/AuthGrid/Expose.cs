using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace APIGateway.AuthGrid
{
    public class Expose : Controller
    {
        private readonly IMediator _mediator;
        public Expose(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(ExposerInterface.AuthAdd)]
        public async Task<IActionResult> AuthAdd([FromBody] AuthSettupCreate comm)
        {
            var response = await _mediator.Send(comm);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpGet(ExposerInterface.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var que = new ExposeAuthGrid();
            return Ok(await _mediator.Send(que));
        }
        [HttpGet(ExposerInterface.GetSiingle)]
        public async Task<IActionResult> GetSiingle([FromQuery] ExposeAnAuthGrid que)
        {
            return Ok(await _mediator.Send(que));
        }
        
        [HttpPost(ExposerInterface.GetTracked)]
        public async Task<IActionResult> GetTracked([FromBody] ExposeTracker query)
        {
            var res = await _mediator.Send(query);
            if (res == HttpStatusCode.OK)
                return Ok();
            if (res == HttpStatusCode.Unauthorized)
                return Unauthorized();
            return Ok();
        }
        [HttpPost(ExposerInterface.FAILED_LOGIN)]
        public async Task<IActionResult> FAILED_LOGIN([FromBody] LoginFailed comm)
        {
            var response = await _mediator.Send(comm);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }
        [HttpPost(ExposerInterface.Session_LOGIN)]
        public async Task<IActionResult> Session_LOGIN([FromBody] SessionTrail comm)
        {
            var response = await _mediator.Send(comm);
            if (response.Status.IsSuccessful)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet(ExposerInterface.GetOtherServiceSetting)]
        public async Task<IActionResult> GetOtherServiceSetting()
        {
            var que = new ExposeOtherServiceAuthGrid();
            return Ok(await _mediator.Send(que));
        }
        

        public class ExposerInterface
        {
            public const string AuthAdd = "/api/v1/admin/auth/guard/add/update";
            public const string GetAll = "/api/v1/admin/auth/guard/get/all";
            public const string GetOtherServiceSetting = "/api/v1/admin/otherservice/auth/guard/get/all";
            public const string GetSiingle = "/api/v1/admin/auth/guard/get/single";
            public const string FAILED_LOGIN = "/api/v1/identity/failed/login";
            public const string Session_LOGIN = "/api/v1/identity/session/login";
            public const string GetTracked = "/api/v1/identity/get/trackedlogin";
        }
    }
}
