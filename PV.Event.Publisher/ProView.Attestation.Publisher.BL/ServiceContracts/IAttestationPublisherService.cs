using System.Threading.Tasks;
using ProView.Attestation.Publisher.Model;

namespace ProView.Attestation.Publisher.BL.ServiceContracts
{
    /// <summary>
    /// Attestation service contract
    /// </summary>
    public interface IAttestationPublisherService
    {
        /// <summary>
        /// Method to publish attestation message
        /// </summary>
        /// <param name="attestationMessage"></param>
        /// <returns>Result</returns>
        void ValidateAttestationMessage(AttestationMessage attestationMessage);

        /// <summary>
        /// Publish attestation message
        /// </summary>
        /// <param name="attestationMessage">attestation message object</param>
        /// <returns>string</returns>
        Task<string> PublishAttestationMessage(AttestationMessage attestationMessage);
    }
}
