﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GODPAPIs.Contracts.GeneralExtension
{
    public class DeleteItem
    {
        public int targetId { get; set; }
    }
    public class MultiDeleteItems
    {
        public List<DeleteItem> targetIds { get; set; }
    }
}
