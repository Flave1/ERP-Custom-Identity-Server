using APIGateway.Contracts.Commands.Company;
using APIGateway.Repository.Interface.Workflow;
using GODPAPIs.Contracts.Commands.Workflow;
using GODPAPIs.Contracts.RequestResponse.Workflow;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Workflow
{
    public class GenerateExportWorkflowGroupCommandHandler : IRequestHandler<GenerateExportWorkflowGroupCommand, byte[]>
    {
        private readonly IWorkflowRepository _repo;
        public GenerateExportWorkflowGroupCommandHandler(IWorkflowRepository repository)
        {
            _repo = repository;
        }
        public async Task<byte[]> Handle(GenerateExportWorkflowGroupCommand request, CancellationToken cancellationToken)
        {
            return await _repo.GenerateExportWorkflowGroupAsync();
        }
    }
}
