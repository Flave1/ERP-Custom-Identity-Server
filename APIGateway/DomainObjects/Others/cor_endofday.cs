namespace GODP.APIsContinuation.DomainObjects.Others
{
    using GODPAPIs.Contracts.GeneralExtension;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class cor_endofday : GeneralEntity
    {
        [Key]
        public int EndOfDayId { get; set; }
         

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }
    }
}
