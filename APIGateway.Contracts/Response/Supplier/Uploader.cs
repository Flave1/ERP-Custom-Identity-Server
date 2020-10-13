using GOSLibraries.GOS_API_Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Response.Supplier
{
    public class FSTempalteUploader 
    {
        public int CompanyStructureId { get; set; }
        public IFormFile File { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
