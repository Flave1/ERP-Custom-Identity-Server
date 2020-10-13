using GODPAPIs.Contracts.GeneralExtension;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Commands.Common
{
    public class UploadCountryCommand : IRequest<FileUploadRespObj>
    {
        public byte[] File { get; set; }
    }
}
