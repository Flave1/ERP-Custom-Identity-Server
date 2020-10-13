using GODPAPIs.Contracts.GeneralExtension;
using GODPAPIs.Contracts.Response.CompanySetup;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Commands.Company
{
    public class UploadFSTemplateCommand : IRequest<CompanyRegRespObj>
    {
        private static string FSTemplateFolder = "~/Files/FSTemplate/";
        private static TimeSpan epochTicks = new TimeSpan(new DateTime(1970, 1, 1).Ticks);
        TimeSpan unixTicks = new TimeSpan(DateTime.UtcNow.Ticks) - epochTicks;
        public UploadFSTemplateCommand()
        {

        }
    }

    public class UploadCompanyStructureCommand : IRequest<FileUploadRespObj> 
    {
        public byte[] File { get; set; }
    }
}
