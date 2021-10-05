using BIT.EfCore.Sync.Data;
using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Collections.Generic;
using System.Text;

namespace BIT.EfCore.Sync
{
    public class EFSyncFrameworkServiceBase : IEFSyncFrameworkService
    {
        protected Dictionary<Type, IUpdateSqlGenerator> _UpdateGenerators;
        protected IEnumerable<DeltaGeneratorBase> _deltaGenerators;
        public EFSyncFrameworkServiceBase(IEnumerable<DeltaGeneratorBase> deltaGenerators)
        {
            _UpdateGenerators = new Dictionary<Type, IUpdateSqlGenerator>();
            _deltaGenerators = deltaGenerators;

        }

        public Dictionary<Type, IUpdateSqlGenerator> UpdateGenerators => _UpdateGenerators;

        public virtual IEnumerable<SqlCommandText> AppendDeleteOperation(ModificationCommand command)
        {
            List<SqlCommandText> SqlCommands = new List<SqlCommandText>();
            foreach (KeyValuePair<Type, IUpdateSqlGenerator> type in UpdateGenerators)
            {
                StringBuilder builder = new StringBuilder();
                type.Value.AppendDeleteOperation(builder, command, 0);
                SqlCommands.Add(new SqlCommandText(builder.ToString(), type.Key.FullName));
            }
            return SqlCommands;
        }

        public IEnumerable<SqlCommandText> AppendInsertOperation(ModificationCommand command)
        {
            List<SqlCommandText> SqlCommands = new List<SqlCommandText>();
            foreach (KeyValuePair<Type, IUpdateSqlGenerator> type in UpdateGenerators)
            {
                StringBuilder builder = new StringBuilder();
                type.Value.AppendInsertOperation(builder, command, 0);
                SqlCommands.Add(new SqlCommandText(builder.ToString(), type.Key.FullName));
            }
            return SqlCommands;
        }

        public IEnumerable<SqlCommandText> AppendUpdateOperation(ModificationCommand command)
        {
            List<SqlCommandText> SqlCommands = new List<SqlCommandText>();
            foreach (KeyValuePair<Type, IUpdateSqlGenerator> type in UpdateGenerators)
            {
                StringBuilder builder = new StringBuilder();
                type.Value.AppendUpdateOperation(builder, command, 0);
                SqlCommands.Add(new SqlCommandText(builder.ToString(), type.Key.FullName));
            }
            return SqlCommands;
        }

        public void RegisterDeltaGenerators(IServiceProvider serviceProvider)
        {
            foreach (DeltaGeneratorBase deltaGeneratorBase in _deltaGenerators)
            {
                var updateSqlGenerator = deltaGeneratorBase.CreateInstance(serviceProvider);
                Type key = updateSqlGenerator.GetType();
                UpdateGenerators.Add(key, updateSqlGenerator);
            }
        }


    }
}
