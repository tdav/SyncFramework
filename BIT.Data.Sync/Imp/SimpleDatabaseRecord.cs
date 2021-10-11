using System;
using System.Linq;

namespace BIT.Data.Sync.TextImp
{
    public class SimpleDatabaseRecord 
    {
        public string Text { get; set; }
        public SimpleDatabaseRecord()
        {

        }

        public Guid Key { get; set; }
    }
}
