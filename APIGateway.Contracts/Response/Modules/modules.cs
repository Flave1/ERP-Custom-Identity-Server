using GOSLibraries.GOS_API_Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIGateway.Contracts.Response.Modules
{
    public class SolutionModuleRegRespObj
    {
        public int SolutionModuleId { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class SolutionModuleRespObj
    {
        public List<SolutionModuleObj> SolutionModules { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class SolutionModuleObj
    {
        public int SolutionModuleId { get; set; }
        public string SolutionName { get; set; }
    }
}
