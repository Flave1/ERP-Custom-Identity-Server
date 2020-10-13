using GODPAPIs.Contracts.GeneralExtension; 
using System.ComponentModel.DataAnnotations; 
namespace GODP.APIsContinuation.DomainObjects.Workflow
{ 
    public partial class cor_workflowgroup : GeneralEntity
    {  
        [Key]
        public int WorkflowGroupId { get; set; }

        [Required]
        [StringLength(250)]
        public string WorkflowGroupName { get; set; } 
    }
}
