using BIT.Data.Sync;
using BIT.Data.Sync.Extensions;
using BIT.Data.Sync.Options;
using BIT.EfCore.Sync.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Update;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BIT.EfCore.Sync
{
    public class EFDeltaProcessor : DeltaProcessorBase
    {
        string providerName;

        string connectionString;
        DbContext _dBContext;
        public EFDeltaProcessor(DbContext dBContext) : base(null)
        {
            _dBContext = dBContext;
        }
        public EFDeltaProcessor(DeltaStoreSettings deltaStoreSettings) : base(deltaStoreSettings)
        {
            ConnectionStringParserService connectionStringParserService = new ConnectionStringParserService(deltaStoreSettings.ConnectionString);
            providerName = connectionStringParserService.GetPartByName("DbProviderFactory");
            connectionStringParserService.RemovePartByName("DbProviderFactory");
            connectionString = connectionStringParserService.GetConnectionString();

            //TODO check provider registration later

            //DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);
        }
        protected virtual IDbCommand CreateDbCommand(string CurrentUpdaterType, IDelta delta, IDbConnection dbConnection, ModificationCommandData modificationCommandData, List<object> Parameters, List<IDbDataParameter> SqlParameters)
        {
            IDbCommand dbCommand = dbConnection.CreateCommand();
            //TODO pass the command that is type of the target Database
            var CurrentCommand = modificationCommandData.SqlCommandTexts.FirstOrDefault(c => string.Compare(c.DbEngine, CurrentUpdaterType, StringComparison.Ordinal) == 0);
            if (CurrentCommand == null)
            {
                throw new Exception($"the delta({delta.Index}-{delta.Identity}-{delta.Epoch}) does not contain information for current database  using the updater type:{CurrentUpdaterType}");
            }
            dbCommand.CommandText = CurrentCommand.Command;
            foreach (Parameters parameters in modificationCommandData.parameters)
            {

                var DbCommandParameter = dbCommand.CreateParameter();
                DbCommandParameter.ParameterName = parameters.Name;
                if (parameters.Value == null)
                {

                    DbCommandParameter.Value = DBNull.Value;
                }
                else
                {
                    DbCommandParameter.Value = parameters.Value;
                }


                SqlParameters.Add(DbCommandParameter);
                Parameters.Add(parameters.Value);

                dbCommand.Parameters.Add(DbCommandParameter);
            }

            return dbCommand;
        }
        protected virtual IDbConnection GetConnection(string ConnectionString)
        {
            //HACK https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/obtaining-a-dbproviderfactory

            // Assume failure.
            DbConnection connection = null;

            // Create the DbProviderFactory and DbConnection.
            if (ConnectionString != null)
            {
                try
                {

                 


                    DbProviderFactory factory =
                      DbProviderFactories.GetFactory(providerName);

                    connection = factory.CreateConnection();
                    connection.ConnectionString = ConnectionString;
                }
                catch (Exception ex)
                {
                    // Set the connection to null if it was created.
                    if (connection != null)
                    {
                        connection = null;
                    }
                    Console.WriteLine(ex.Message);
                }
            }
            // Return the connection.
            return connection;
        }
        public override async Task ProcessDeltasAsync(IEnumerable<IDelta> Deltas, CancellationToken cancellationToken)
        {
            foreach (IDelta delta in Deltas)
            {
                IDbConnection dbConnection;
                if (this._dBContext == null)
                {

                    dbConnection = this.GetConnection(this.connectionString);
                }
                else
                {

                    var ServiceProvider = this._dBContext as IInfrastructure<IServiceProvider>;
                    var IUpdateSqlGenerator = ServiceProvider.GetService<IUpdateSqlGenerator>();
                    dbConnection = this._dBContext.Database.GetDbConnection();
                    this.providerName = IUpdateSqlGenerator.GetType().FullName;
                }

                List<ModificationCommandData> ModificationsData = this.GetDeltaOperations<List<ModificationCommandData>>(delta);
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    foreach (ModificationCommandData modificationCommandData in ModificationsData)
                    {
                        foreach (SqlCommandText sqlCommandText in modificationCommandData.SqlCommandTexts)
                        {
                            Debug.WriteLine($"{sqlCommandText.DbEngine}-{sqlCommandText.Command}");
                        }
                        foreach (Parameters parameters in modificationCommandData.parameters)
                        {
                            Debug.WriteLine($"{parameters.Name} : {parameters.Value}");
                        }

                    }
                }


                foreach (ModificationCommandData modificationCommandData in ModificationsData)
                {
                    List<object> Parameters = new List<object>();
                    List<IDbDataParameter> SqlParameters = new List<IDbDataParameter>();
                    IDbCommand dbCommand = CreateDbCommand(providerName, delta, dbConnection, modificationCommandData, Parameters, SqlParameters);
                    dbConnection.Open();
                    var dbCommandAsync = dbCommand as DbCommand;
                    if (dbCommandAsync != null)
                        await dbCommandAsync.ExecuteNonQueryAsync().ConfigureAwait(false);
                    else
                        dbCommand.ExecuteNonQuery();
                    dbConnection.Close();

                }
            }


        }

    }
}