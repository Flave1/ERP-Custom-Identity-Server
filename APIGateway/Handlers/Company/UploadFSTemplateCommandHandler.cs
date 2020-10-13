using APIGateway.Contracts.Commands.Company;
using GODPAPIs.Contracts.Response.CompanySetup;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APIGateway.Handlers.Ccompany
{
    public class UploadFSTemplateCommandHandler : IRequestHandler<UploadFSTemplateCommand, CompanyRegRespObj>
    {
        public Task<CompanyRegRespObj> Handle(UploadFSTemplateCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
