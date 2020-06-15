using System.Threading.Tasks;
using ProView.Attestation.Publisher.Model;

namespace ProView.Attestation.Publisher.BL.ServiceContracts
{
    /// <summary>
    /// Interface for Message Log service
    /// </summary>
    public interface IMessageLogService
    {
        /// <summary>
        /// Message Logging
        /// </summary>
        /// <param name="message">Message object</param>
        /// <returns>true/false</returns>
        Task<bool> LogFailedMessageAsync(LogFailedMessage message);
    }

}
