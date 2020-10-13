namespace GODP.APIsContinuation.DomainObjects.Others
{
    using GODPAPIs.Contracts.GeneralExtension;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class cor_errorlog : GeneralEntity
    {
        [Key]
        public int ErrorLogId { get; set; }

        [StringLength(255)]
        public string Username { get; set; }

        [StringLength(500)]
        public string ApiEndPoint { get; set; }

        [StringLength(255)]
        public string ErrorPath { get; set; }

        [StringLength(500)]
        public string ErrorSource { get; set; }

        [StringLength(1000)]
        public string ErrorMessage { get; set; }

        [StringLength(255)]
        public string ErrorType { get; set; }

        [StringLength(50)]
        public string StatusCode { get; set; }

        public string AllXML { get; set; }

        public DateTime? TimeUTC { get; set; }
    }
}
