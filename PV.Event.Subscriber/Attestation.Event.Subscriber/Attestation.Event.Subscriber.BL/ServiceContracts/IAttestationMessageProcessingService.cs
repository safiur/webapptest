using System.Threading.Tasks;
using Attestation.Event.Subscriber.DAL.Entities;

namespace Attestation.Event.Subscriber.BL.ServiceContracts
{
    /// <summary>
    /// Interface for Message service
    /// </summary>
    public interface IAttestationMessageProcessingService
    {

        /// <summary>
        /// Add Message
        /// </summary>
        /// <param name="message">Message object</param>
        /// <returns></returns>
        Task<int> Add(AttestationMessage message);
        
    }

}
