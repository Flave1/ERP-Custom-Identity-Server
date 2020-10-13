namespace GODP.APIsContinuation.DomainObjects.UserAccount
{
    using GODPAPIs.Contracts.GeneralExtension;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class cor_activityparent : GeneralEntity
    {
        public cor_activityparent()
        {
            cor_activity = new HashSet<cor_activity>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ActivityParentId { get; set; }

        [Required]
        [StringLength(256)]
        public string ActivityParentName { get; set; }
        public virtual ICollection<cor_activity> cor_activity { get; set; }
    }
}
