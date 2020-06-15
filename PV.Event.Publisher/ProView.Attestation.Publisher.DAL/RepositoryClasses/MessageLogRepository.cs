using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using ProView.Attestation.Publisher.DAL.RepositoryContracts;
using PV.Event.Constants;
using Serilog;

namespace ProView.Attestation.Publisher.DAL.RepositoryClasses
{
    /// <summary>
    /// Message Repository implementation
    /// </summary>
    public class MessageLogRepository<TEntity> : IMessageLogRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Database connection
        /// </summary>
        private readonly IConfiguration _config;


        private IDbConnection Connection => new SqlConnection(_config[AppConstants.ConnectionStringsDefaultConnectionString]);

        /// <summary>
        /// MessageRepository Constructor
        /// </summary>
        /// <param name="configuration">Configuration</param>
        public MessageLogRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        /// <summary>
        /// Save the message
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> LogFailedMessageAsync(TEntity entity)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    Log.Logger.Information("{Method} Function started", "LogFailedMessageAsync");
                    Log.Logger.Information("Failed Message content {@entity}", entity);
                    var result = await conn.InsertAsync(entity);
                    Log.Logger.Information("Logged failed message successful for {@entity}", entity);
                    return result;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return await Task.FromResult(default(int));
            }
        }
    }
}
