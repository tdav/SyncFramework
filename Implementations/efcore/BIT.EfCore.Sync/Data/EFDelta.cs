

using BIT.Data.Sync;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIT.EfCore.Sync.Data
{

    public class EFDelta : Delta, IDelta
    {
        public EFDelta()
        {

        }

        public EFDelta(string identity, byte[] operation, bool processed) : base(identity, operation, processed)
        {

        }

        public EFDelta(IDelta Delta) : base(Delta)
        {

        }

        public EFDelta(string identity, Guid oid, byte[] operation, bool processed) : base(identity, oid, operation, processed)
        {

        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public Guid Oid { get; set; }
    }

}
