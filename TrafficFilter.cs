using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace SecurityModule
{
    public class IpTracker
    {
        public string Address { get; set; }
        public int RequestCount { get; set; }
        public DateTime FirstRequest { get; set; }
        public DateTime LastRequest { get; set; }
        public bool IsBlocked { get; set; }
    }
}