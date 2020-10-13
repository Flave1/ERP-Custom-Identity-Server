namespace GODP.APIsContinuation.DomainObjects.UserAccount
{
    using GODPAPIs.Contracts.GeneralExtension; 
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class cor_activity : GeneralEntity
    {
        public cor_activity()
        {
            cor_userroleactivity = new HashSet<cor_userroleactivity>();
            cor_userroleadditionalactivity = new HashSet<cor_userroleadditionalactivity>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ActivityId { get; set; }

        public int ActivityParentId { get; set; }

        [Required]
        [StringLength(256)]
        public string ActivityName { get; set; }

        public virtual cor_activityparent cor_activityparent { get; set; }

        public virtual ICollection<cor_userroleactivity> cor_userroleactivity { get; set; }

        public virtual ICollection<cor_userroleadditionalactivity> cor_userroleadditionalactivity { get; set; }
    }
}
