using FluentValidation;
using GODPAPIs.Contracts.Commands.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Validations.Workflow
{
    public class AddUpdateWorkflowGroupCommandVal : AbstractValidator<AddUpdateWorkflowGroupCommand>
    {
        public AddUpdateWorkflowGroupCommandVal()
        {
            RuleFor(x => x.WorkflowGroupName).NotEmpty().MinimumLength(2);
        }
    }
}
