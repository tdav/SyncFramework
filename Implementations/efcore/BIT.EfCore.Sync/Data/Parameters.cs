using System;

namespace BIT.EfCore.Sync.Data
{
    [Serializable]
    public class Parameters
    {

        public Parameters()
        {

        }
        public string Name { get; set; }
        public object Value { get; set; }


    }
}
