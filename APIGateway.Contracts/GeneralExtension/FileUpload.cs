using GOSLibraries.GOS_API_Response;
using Microsoft.AspNetCore.Http;
namespace GODPAPIs.Contracts.GeneralExtension
{
    public class FileUploadObj
    {
        public IFormFile File { get; set; }    
    }

    public class FileUploadRespObj
    {
        public APIResponseStatus Status { get; set; }
    }
}