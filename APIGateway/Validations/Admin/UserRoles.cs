using APIGateway.Data;
using FluentValidation;
using GODPAPIs.Contracts.Commands.Admin;
using GODPAPIs.Contracts.Response.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Validations.Admin
{
    public  class AddUpdateUserRoleActivityCommandVal : AbstractValidator<AddUpdateUserRoleActivityCommand>
    {
         
    }
}
