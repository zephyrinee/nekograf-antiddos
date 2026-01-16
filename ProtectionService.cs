using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace SecurityModule
{
    public class ProtectionService : IDisposable
    {
        private readonly Dictionary<string, IpTracker> ipTrackers;
        private readonly RequestValidator validator;
        private readonly Timer cleanupTimer;
        private readonly object syncLock;
        private bool isDisposed;
        
        public event Action<string> OnIpBlocked;
        public event Action<string> OnIpUnblocked;
        
        public ProtectionService()
        {
            ipTrackers = new Dictionary<string, IpTracker>();
            validator = new RequestValidator();
            syncLock = new object();
            cleanupTimer = new Timer(CleanupExpiredRecords, null, 300000, 300000);
        }
        
        public bool ProcessRequest(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                return false;
                
            lock (syncLock)
            {
                if (!ipTrackers.ContainsKey(ipAddress))
                {
                    ipTrackers[ipAddress] = new IpTracker
                    {
                        Address = ipAddress,
                        RequestCount = 1,
                        FirstRequest = DateTime.Now,
                        LastRequest = DateTime.Now,
                        IsBlocked = false
                    };
                    return true;
                }
                
                var tracker = ipTrackers[ipAddress];
                tracker.LastRequest = DateTime.Now;
                
                var allowed = validator.IsRequestAllowed(ipAddress, ipTrackers);
                
                if (!allowed && !tracker.IsBlocked)
                {
                    tracker.IsBlocked = true;
                    OnIpBlocked?.Invoke(ipAddress);
                }
                
                return allowed;
            }
        }
        
        private void CleanupExpiredRecords(object state)
        {
            lock (syncLock)
            {
                var expiredIps = new List<string>();
                var cutoffTime = DateTime.Now.AddMinutes(-60);
                
                foreach (var kvp in ipTrackers)
                {
                    if (kvp.Value.LastRequest < cutoffTime && !kvp.Value.IsBlocked)
                    {
                        expiredIps.Add(kvp.Key);
                    }
                }
                
                foreach (var ip in expiredIps)
                {
                    ipTrackers.Remove(ip);
                }
            }
        }
        
        public List<string> GetBlockedIps()
        {
            lock (syncLock)
            {
                var blocked = new List<string>();
                foreach (var kvp in ipTrackers)
                {
                    if (kvp.Value.IsBlocked)
                    {
                        blocked.Add(kvp.Key);
                    }
                }
                return blocked;
            }
        }
        
        public void UnblockIp(string ipAddress)
        {
            lock (syncLock)
            {
                if (ipTrackers.ContainsKey(ipAddress))
                {
                    ipTrackers[ipAddress].IsBlocked = false;
                    ipTrackers[ipAddress].RequestCount = 0;
                    OnIpUnblocked?.Invoke(ipAddress);
                }
            }
        }
        
        public void Dispose()
        {
            if (!isDisposed)
            {
                cleanupTimer?.Dispose();
                isDisposed = true;
            }
        }
    }
}