using BIT.Data.Sync.Guids.CombProvider;

using System;

namespace BIT.Data.Sync
{
    /// <summary>
    /// An implementation of the IDelta interface, this class is primary used for serialization and transportation purpose 
    /// </summary>
    public class Delta : IDelta
    {
        public Delta()
        {
        }
        public static Guid GenerateComb()
        {
            return GuidService.Create();
        }
        public Delta(string identity, byte[] operation)
        {

            Identity = identity;
            Operation = operation;
          
        }
        public Delta(IDelta Delta)
        {

            Identity = Delta.Identity;
            Index = Delta.Index;
            Operation = Delta.Operation;
          

        }
        public Delta(string identity, Guid index, byte[] operation)
        {

            Identity = identity;
            Index = index;
            Operation = operation;
         
        }
        public virtual DateTime Date { get; set; }
        public virtual string Identity { get; set; }

        public virtual Guid Index { get; set; }

        public virtual byte[] Operation { get; set; }
     
        public virtual double Epoch { get; set; }

    }
}