using System;
using System.Collections.Generic;

namespace BIT.EfCore.Sync.Data
{
    [Serializable]
    public class ModificationCommandData
    {

        public ModificationCommandData(List<Parameters> parameters, List<SqlCommandText> sqlCommandTexts)
        {

            this.parameters = parameters;
            this.SqlCommandTexts = sqlCommandTexts;

        }
        public ModificationCommandData(IEnumerable<Parameters> parameters, IEnumerable<SqlCommandText> sqlCommandTexts)
        {

            this.parameters = new List<Parameters>(parameters);
            this.SqlCommandTexts = new List<SqlCommandText>(sqlCommandTexts);

        }
        public ModificationCommandData()
        {
            parameters = new List<Parameters>();
            SqlCommandTexts = new List<SqlCommandText>();
        }
        public List<Parameters> parameters { get; set; }
        public List<SqlCommandText> SqlCommandTexts { get; set; }

    }
}
