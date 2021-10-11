using System;
using System.Linq;

namespace BIT.Data.Sync.TextImp
{
    public class TextRecord : IRecord
    {
        public string Text { get; set; }
        public TextRecord()
        {

        }

        public Guid Key { get; set; }
    }
}
