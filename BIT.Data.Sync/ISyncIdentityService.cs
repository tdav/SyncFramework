using System;
using System.Collections.Generic;
using System.Text;

namespace BIT.Data.Sync
{
    public interface ISyncIdentityService
    {
        string Identity { get; set; }
    }
    public class SyncIdentityService: ISyncIdentityService
    {
        
        public SyncIdentityService()
        {
            
        }
        public SyncIdentityService(string identity)
        {
            Identity = identity;
        }

        public string Identity { get; set; }
    }
}
