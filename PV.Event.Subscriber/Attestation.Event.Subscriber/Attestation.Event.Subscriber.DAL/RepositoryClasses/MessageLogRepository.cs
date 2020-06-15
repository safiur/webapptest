using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Attestation.Event.Subscriber.DAL.RepositoryContracts;
using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using PV.Event.Constants;
using Serilog;

namespace Attestation.Event.Subscriber.DAL.RepositoryClasses
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
        public async Task<int> LogPublishedMessageAsync(TEntity entity)
        {
            Log.Logger.Information("{Method} Function started", "LogPublishedMessageAsync");
            
            using (IDbConnection conn = Connection)
            {
                var result = await conn.InsertAsync<TEntity>(entity);
                Log.Logger.Information("Logged message successful for {@entity}", entity);
                return result;
            }

        }
    }
}
