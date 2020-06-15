using System.Threading.Tasks;
using Attestation.Event.Subscriber.BL.ServiceContracts;
using Attestation.Event.Subscriber.DAL.Entities;
using Attestation.Event.Subscriber.DAL.RepositoryContracts;

namespace Attestation.Event.Subscriber.BL.ServiceClasses
{
    /// <summary>
    /// Message service implementation
    /// </summary>
    public class AttestationMessageProcessingService : IAttestationMessageProcessingService
    {
        private readonly IMessageLogRepository<AttestationMessage> _messageRepo;

        public AttestationMessageProcessingService(IMessageLogRepository<AttestationMessage> messageRepo)
        {
            _messageRepo = messageRepo;
        }

        public async Task<int> Add(AttestationMessage message)
        {
            return await _messageRepo.LogPublishedMessageAsync(message);
        }

    }
}
