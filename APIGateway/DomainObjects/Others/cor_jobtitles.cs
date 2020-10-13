namespace GODP.APIsContinuation.DomainObjects.Others
{
    using GODPAPIs.Contracts.GeneralExtension;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class cor_jobtitles : GeneralEntity
    {
        [Key]
        public int JobTitleId { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string JobDescription { get; set; }

        [StringLength(1000)]
        public string Skills { get; set; }

        [StringLength(2000)]
        public string SkillDescription { get; set; }
    }
}
