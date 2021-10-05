using BIT.EfCore.Sync.Data;
using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Collections.Generic;

namespace BIT.EfCore.Sync
{
    public interface IEFSyncFrameworkService
    {
        IEnumerable<SqlCommandText> AppendDeleteOperation(ModificationCommand command);
        IEnumerable<SqlCommandText> AppendInsertOperation(ModificationCommand command);
        IEnumerable<SqlCommandText> AppendUpdateOperation(ModificationCommand command);
        Dictionary<Type, IUpdateSqlGenerator> UpdateGenerators { get; }
        //void RegisterUpdater(IUpdateSqlGenerator updateSqlGenerator);
        void RegisterDeltaGenerators(IServiceProvider serviceProvider);
    }
}
