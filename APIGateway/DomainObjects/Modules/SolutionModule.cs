using GODPAPIs.Contracts.GeneralExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.DomainObjects.Modules
{
    public class SolutionModule : GeneralEntity
    {
        public int SolutionModuleId { get; set; }
        public string SolutionName { get; set; }
    }
}
