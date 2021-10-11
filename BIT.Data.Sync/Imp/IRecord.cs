using System;
using System.Linq;

namespace BIT.Data.Sync.TextImp
{
    public interface IRecord
    {
        Guid Key { get; set; }
    }
}
