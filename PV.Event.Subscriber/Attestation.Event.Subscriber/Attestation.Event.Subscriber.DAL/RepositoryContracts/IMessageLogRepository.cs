using System.Threading.Tasks;

namespace Attestation.Event.Subscriber.DAL.RepositoryContracts
{
    /// <summary>
    /// Interface Message Repository contract
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IMessageLogRepository<in TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Save the message
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> LogPublishedMessageAsync(TEntity entity);
    }
}