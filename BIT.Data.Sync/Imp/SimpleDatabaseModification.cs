using System;
using System.Linq;

namespace BIT.Data.Sync.TextImp
{
    public class SimpleDatabaseModification
    {
        public OperationType Operation { get; set; }
        public SimpleDatabaseModification(OperationType operation, SimpleDatabaseRecord record)
        {
            Operation = operation;
            Record = record;
        }
        public SimpleDatabaseRecord Record { get; set; }
    }
}
