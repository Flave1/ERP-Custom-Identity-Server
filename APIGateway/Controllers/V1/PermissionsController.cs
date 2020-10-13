using APIGateway.ActivityRequirement;
using APIGateway.Handlers.Permissions;
using GODPAPIs.Contracts.V1;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Controllers.V1
{
    public class PermissionsController : Controller
    {
        private readonly IMediator _mediator;
        public PermissionsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
    }
}
