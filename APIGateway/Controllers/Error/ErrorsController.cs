﻿using GOSLibraries.GOS_Error_logger.Service;
using GOSLibraries.GOS_API_Response;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 
using Newtonsoft.Json; 
using System.Net; 
using GOSLibraries.GOS_Error_logger;

namespace GODP.APIsContinuation.CustomError
{
    [Route("/errors")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : Controller
    {
        private readonly ILoggerService _logger;
        public ErrorsController(ILoggerService loggerService)
        {
            _logger = loggerService;
        }
        [Route("{code}")]
        public IActionResult GenerateErrorMessage(int? code)
        {
            BaseApiError error;
            HttpContext context = HttpContext;
            HttpStatusCode parseCode = (HttpStatusCode)code;

            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature != null)
            {
                var exception = exceptionHandlerPathFeature.Error;

                var result = JsonConvert.SerializeObject(exception.Message);

                context.Response.ContentType = "application/json";

                if (parseCode == HttpStatusCode.NotFound)
                {
                    error = new BaseApiError(code ?? 0, parseCode.ToString(), "Use A Valid URL");
                    _logger.Error($"ErrorsController {error.Technical_Message}");
                    return new ObjectResult (new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = error.Friendly_Message,
                            MessageId = error.StatusDescription,
                            TechnicalMessage = error.Technical_Message
                        }
                    });
                }
                if (parseCode == HttpStatusCode.InternalServerError)
                {
                    error = new BaseApiError(code ?? 0, parseCode.ToString(), "Unable to process Request! Please try again later, The server encountered an internal error or misconfiguration ", result);
                    _logger.Error($"ErrorsController {error.Technical_Message}");
                    return new ObjectResult(new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = error.Friendly_Message,
                            MessageId = error.StatusDescription,
                            TechnicalMessage = error.Technical_Message
                        }
                    });
                }

                if (parseCode == HttpStatusCode.Forbidden)
                {
                    error = new BaseApiError(code ?? 0, parseCode.ToString(), "You do not have enough permission to perform operation", result);
                    _logger.Error($"ErrorsController {error.Technical_Message}");
                    return new ObjectResult(new APIResponseStatus
                    {
                        Message = new APIResponseMessage
                        {
                            FriendlyMessage = error.Friendly_Message,
                            MessageId = error.StatusDescription,
                            TechnicalMessage = error.Technical_Message
                        }
                    });
                }

                error = new BaseApiError(code ?? 0, parseCode.ToString(), result);
                _logger.Error($"ErrorsController {error.Technical_Message}");
                return new ObjectResult(new APIResponseStatus
                {
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = error.Friendly_Message,
                        MessageId = error.StatusDescription,
                        TechnicalMessage = error.Technical_Message
                    }
                });
            }
            if (parseCode == HttpStatusCode.NotFound)
            {
                error = new BaseApiError(code ?? 0, parseCode.ToString(), "Use A Valid URL");
                _logger.Error($"ErrorsController {error.Technical_Message}");
                return new ObjectResult(new APIResponseStatus
                {
                    Message = new APIResponseMessage
                    {
                        FriendlyMessage = error.Friendly_Message,
                        MessageId = error.StatusDescription,
                        TechnicalMessage = error.Technical_Message
                    }
                });
            }
            error = new BaseApiError(code ?? 0, parseCode.ToString());
            return new ObjectResult(new APIResponseStatus
            {
                Message = new APIResponseMessage
                {
                    FriendlyMessage = error.Friendly_Message,
                    MessageId = error.StatusDescription,
                    TechnicalMessage = error.Technical_Message
                }
            });
        }




    }
}
