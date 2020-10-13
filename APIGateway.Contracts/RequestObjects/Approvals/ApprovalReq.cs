using GODPAPIs.Contracts.GeneralExtension;
using System;
using System.Collections.Generic;
using System.Text;

namespace GODPAPIs.Contracts.RequestObjects.Approvals
{
    public class ApprovalReq : GeneralEntity
    {
        public int OperationId { get; set; }
        public int TargetId { get; set; }
        public short ApprovalStatusId { get; set; }
        public string Comment { get; set; }
        public string OperationURL { get; set; }
        public int MyLevelId { get; set; }
        public int NextLevelId { get; set; }
        public decimal Amount { get; set; }
        public bool ExternalInitialization { get; set; }
        public bool KeepPending { get { return true; } }
        public bool DeferredExecution { get; set; }
        public int StaffId { get; set; }
    }
}
