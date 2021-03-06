using BIT.EfCore.Sync;
using Microsoft.EntityFrameworkCore.SqlServer.Update.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using System;
namespace SyncFrameworkTests.EF.SqlServer
{
    public class SqlServerDeltaGenerator : DeltaGeneratorBase
    {

        public SqlServerDeltaGenerator()
        {

        }

        public override IUpdateSqlGenerator CreateInstance(IServiceProvider serviceProvider)
        {
            ISqlGenerationHelper ISqlGenerationHelper = serviceProvider.GetService(typeof(ISqlGenerationHelper)) as ISqlGenerationHelper;
            IRelationalTypeMappingSource IRelationalTypeMappingSource = serviceProvider.GetService(typeof(IRelationalTypeMappingSource)) as IRelationalTypeMappingSource;
            UpdateSqlGeneratorDependencies updateSqlGeneratorDependencies = new UpdateSqlGeneratorDependencies(ISqlGenerationHelper, IRelationalTypeMappingSource);
            SqlServerUpdateSqlGenerator sqlServerUpdateSqlGenerator = new SqlServerUpdateSqlGenerator(updateSqlGeneratorDependencies);
            //IEFSyncFrameworkService eFSyncFrameworkService = serviceProvider.GetService(typeof(IEFSyncFrameworkService)) as IEFSyncFrameworkService;
            //eFSyncFrameworkService.RegisterUpdater(sqlServerUpdateSqlGenerator);
            return sqlServerUpdateSqlGenerator;
        }
    }
}
