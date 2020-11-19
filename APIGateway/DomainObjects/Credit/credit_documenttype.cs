using GODPAPIs.Contracts.GeneralExtension; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGateway.DomainObjects.Credit
{
    public partial class credit_documenttype : GeneralEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DocumentTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } 


    }
}
