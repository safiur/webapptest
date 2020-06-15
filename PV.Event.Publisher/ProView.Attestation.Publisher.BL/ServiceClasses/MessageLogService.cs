using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProView.Attestation.Publisher.BL.ServiceContracts;
using ProView.Attestation.Publisher.DAL.Entities;
using ProView.Attestation.Publisher.DAL.RepositoryContracts;
using ProView.Attestation.Publisher.Model;
using Serilog;

namespace ProView.Attestation.Publisher.BL.ServiceClasses
{
    /// <summary>
    /// Message Log service implementation
    /// </summary>
    public class MessageLogService : IMessageLogService
    {
        private readonly IMessageLogRepository<FailedAttestationMessage> _messageLogRepo;
        private readonly IConfiguration _configuration;
        private readonly ILogger<MessageLogService> _logger;

        public MessageLogService(IMessageLogRepository<FailedAttestationMessage> messageLogRepo, IConfiguration configuration,
            ILogger<MessageLogService> logger)
        {
            _messageLogRepo = messageLogRepo;
            _configuration = configuration;
            _logger = logger;
        }


        /// <summary>
        /// Log the failed message
        /// </summary>
        /// <param name="message">Attestation failed message</param>
        /// <returns>true/false</returns>
        public async Task<bool> LogFailedMessageAsync(LogFailedMessage message)
        {
            try
            {
                Log.Logger.Information("{Method} Function started", "LogFailedMessageAsync");
                Log.Logger.Information("Failed Message content {@message}", message);

                FailedAttestationMessage serviceBusMessage = new FailedAttestationMessage
                {
                    CaqhId = message.CAQHId,
                    AttestationId = message.AttestationId,
                    AttestationJsonId = message.AttestationJsonId,
                    AttestationDateTimeStamp = message.AttestationDateTimeStamp,
                    ErrorMessage = message.Error,
                    ErrorDetail = message.ErrorDetail,
                    CreatedDate = DateTime.Now,
                    
                };

                var result = await _messageLogRepo.LogFailedMessageAsync(serviceBusMessage);
                Log.Logger.Information("Logged failed message successful for {@message}", message);
                return result == 0;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }
    }
}
