using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SecurityModule
{
    public class RequestValidator
    {
        private readonly int maxRequestsPerMinute;
        private readonly int blockDurationMinutes;
        
        public RequestValidator(int maxRequests = 100, int blockDuration = 30)
        {
            maxRequestsPerMinute = maxRequests;
            blockDurationMinutes = blockDuration;
        }
        
        public bool IsRequestAllowed(string ipAddress, Dictionary<string, IpTracker> trackers)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                return false;
                
            if (!trackers.ContainsKey(ipAddress))
                return true;
                
            var tracker = trackers[ipAddress];
            
            if (tracker.IsBlocked)
            {
                var blockTime = DateTime.Now - tracker.LastRequest;
                if (blockTime.TotalMinutes > blockDurationMinutes)
                {
                    tracker.IsBlocked = false;
                    tracker.RequestCount = 0;
                    return true;
                }
                return false;
            }
            
            var timeWindow = DateTime.Now - tracker.FirstRequest;
            if (timeWindow.TotalMinutes >= 1)
            {
                tracker.RequestCount = 1;
                tracker.FirstRequest = DateTime.Now;
                return true;
            }
            
            if (tracker.RequestCount >= maxRequestsPerMinute)
            {
                tracker.IsBlocked = true;
                return false;
            }
            
            tracker.RequestCount++;
            return true;
        }
    }
}