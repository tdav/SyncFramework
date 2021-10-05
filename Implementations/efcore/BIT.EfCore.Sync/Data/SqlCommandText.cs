using System;

namespace BIT.EfCore.Sync.Data
{
    [Serializable]
    public class SqlCommandText
    {

        public string Command { get; set; }
        public string DbEngine { get; set; }
        public SqlCommandText(string command, string dbEngine)
        {
            Command = command;
            DbEngine = dbEngine;
        }
        public SqlCommandText()
        {
        }
    }
}
