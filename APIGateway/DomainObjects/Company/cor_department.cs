namespace GODP.APIsContinuation.DomainObjects.Company
{
    using GODPAPIs.Contracts.GeneralExtension;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class cor_department : GeneralEntity
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(10)]
        public string DepartmentCode { get; set; }

        [Required]
        [StringLength(250)]
        public string DepartmentName { get; set; }

        public int BranchId { get; set; }

        public virtual cor_branch cor_branch { get; set; }
    }
}
