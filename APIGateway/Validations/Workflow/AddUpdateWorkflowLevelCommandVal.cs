using APIGateway.Contracts.Commands.Workflow;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Validations.Workflow
{
    public class AddUpdateWorkflowLevelCommandVal : AbstractValidator<AddUpdateWorkflowLevelCommand>
    {
        public AddUpdateWorkflowLevelCommandVal()
        {
            RuleFor(c => c.Position).NotEmpty();
            RuleFor(c => c.RoleId).NotEmpty();
            RuleFor(c => c.WorkflowGroupId).NotEmpty();
            RuleFor(c => c.WorkflowLevelName).NotEmpty(); 
        }
    }
}
