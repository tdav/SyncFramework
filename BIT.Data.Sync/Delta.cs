﻿using BIT.Data.Sync.Guids.StaticProviders;
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
            return Provider.PostgreSql.Create();
           
        }
        public Delta(string identity, byte[] operation, bool processed = false)
        {

            Identity = identity;
            Operation = operation;
            Processed = processed;
        }
        public Delta(IDelta Delta)
        {

            Identity = Delta.Identity;
            Index = Delta.Index;
            Operation = Delta.Operation;
            Processed = Delta.Processed;
            Epoch = Delta.Epoch;
            Date = Delta.Date;
        }
        public Delta(string identity, Guid index, byte[] operation, bool processed = false)
        {

            Identity = identity;
            Index = index;
            Operation = operation;
            Processed = processed;
        }
        public virtual DateTime Date { get; set; }
        public virtual string Identity { get; set; }

        public virtual Guid Index { get; set; }

        public virtual byte[] Operation { get; set; }
        public virtual bool Processed { get; set; }
        public virtual double Epoch { get; set; }

    }
}