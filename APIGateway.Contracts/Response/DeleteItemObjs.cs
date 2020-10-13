﻿using GOSLibraries.GOS_API_Response;
using System.Collections.Generic;

namespace GODPAPIs.Contracts.RequestResponse
{
    public class DeleteItemReqObj
    {
        public int TargetId { get; set; }
    }
    public class MultiDeleteItemsReqObj
    {
        public List<DeleteItemReqObj> TargetIds { get; set; }
    }

    public class DeleteRespObj
    {
        public bool Deleted { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
