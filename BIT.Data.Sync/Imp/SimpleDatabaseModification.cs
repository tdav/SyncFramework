using System;
using System.Linq;

namespace BIT.Data.Sync.TextImp
{
    public class SimpleDatabaseModification
    {
        public Operation Operation { get; set; }
        public SimpleDatabaseModification(Operation operation, IRecord record)
        {
            Operation = operation;
            Record = record;
        }
        public IRecord Record { get; set; }
    }
}
