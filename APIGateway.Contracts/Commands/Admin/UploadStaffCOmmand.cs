using GODPAPIs.Contracts.GeneralExtension;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Commands.Admin
{
    public class UploadStaffCommand : IRequest<FileUploadRespObj> 
    { 
        public byte[] File { get; set; }
    }
}
