using GODPAPIs.Contracts.GeneralExtension; 
using System.ComponentModel.DataAnnotations; 

namespace APIGateway.DomainObjects.Credit
{
    public partial class credit_documenttype : GeneralEntity
    {

        [Key]
        public int DocumentTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } 


    }
}
