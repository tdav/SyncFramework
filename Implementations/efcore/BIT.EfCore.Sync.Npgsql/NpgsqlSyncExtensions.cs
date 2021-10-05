
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Npgsql.EntityFrameworkCore.PostgreSQL.Update.Internal;
using System;
namespace Microsoft.Extensions.DependencyInjection
{
    public static class NpgsqlSyncExtensions
    {
        public static IServiceProvider WithNpgsqlDeltaGenerator(this IServiceProvider serviceProvider)
        {

            ISqlGenerationHelper ISqlGenerationHelper = serviceProvider.GetService(typeof(ISqlGenerationHelper)) as ISqlGenerationHelper;
            IRelationalTypeMappingSource IRelationalTypeMappingSource = serviceProvider.GetService(typeof(IRelationalTypeMappingSource)) as IRelationalTypeMappingSource;
            UpdateSqlGeneratorDependencies updateSqlGeneratorDependencies = new UpdateSqlGeneratorDependencies(ISqlGenerationHelper, IRelationalTypeMappingSource);
#pragma warning disable EF1001 // Internal EF Core API usage.
            //NpgsqlUpdateSqlGenerator NpgsqlUpdateSqlGenerator = new NpgsqlUpdateSqlGenerator(updateSqlGeneratorDependencies);
#pragma warning restore EF1001 // Internal EF Core API usage.
            //IEFSyncFrameworkService eFSyncFrameworkService = serviceProvider.GetService(typeof(IEFSyncFrameworkService)) as IEFSyncFrameworkService;

            //eFSyncFrameworkService.RegisterUpdater(NpgsqlUpdateSqlGenerator);

            return serviceProvider;

        }
    }

}
