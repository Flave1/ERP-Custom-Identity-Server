using GODPAPIs.Contracts.GeneralExtension; 
using System.ComponentModel.DataAnnotations; 

namespace APIGateway.DomainObjects.Credit
{
    public partial class cor_identification : GeneralEntity
    { 

        [Key]
        public int IdentificationId { get; set; }

        [Required]
        [StringLength(250)]
        public string IdentificationName { get; set; } 
         
    }
}
