using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Newtonsoft.Json;
using ProView.Attestation.Publisher.BL.ServiceContracts;
using ProView.Attestation.Publisher.Model;
using PV.Event.Common;
using PV.Event.Constants;
using Serilog;

namespace ProView.Attestation.Publisher.BL.ServiceClasses
{
    /// <summary>
    /// Attestation service class
    /// </summary>
    public class AttestationPublisherService : IAttestationPublisherService
    {
        /// <summary>
        /// _config service to fetch application configuration
        /// </summary>
        private static IConfiguration _config;
        private readonly IMessageLogService _messageLogService;

        /*
                private IAttestationPublisherService _attestationPublisherServiceImplementation;
        */

        /// <summary>
        /// Attestation Publisher Service constructor
        /// </summary>
        /// <param name="config">config service to fetch application configuration</param>
        /// <param name="messageLogService"></param>
        public AttestationPublisherService(IConfiguration config, IMessageLogService messageLogService)
        {
            _config = config;
            _messageLogService = messageLogService;
        }

        public AttestationPublisherService(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Method to publish attestation message
        /// </summary>
        /// <param name="attestationMessage">Attestation Message Object</param>
        /// <returns>Message Id</returns>
        public async Task<string> PublishAttestationMessage(AttestationMessage attestationMessage)
        {
            Log.Logger.Information("PublishAttestationMessage Function Started");
            
            Log.Logger.Information("ValidateAttestationMessage Function Started");
            
            string messageBody = JsonConvert.SerializeObject(attestationMessage);
            string sbConnectionString = _config[AppConstants.AzureSettingsServiceBusConnectionString];
            string sbTopic = _config[AppConstants.AzureSettingsServiceBusTopic];
            var retryCount = int.Parse( _config[AppConstants.AzureSettingsMaxPublisherRetryCount]);
            ITopicClient topicClient = null;
            string messageId;
            try
            {
                Log.Logger.Information("PublishAttestationMessage Create connection with Topic");
                //Create connection with Topic
                topicClient = new TopicClient(sbConnectionString, sbTopic);
                var message = new Message(Encoding.UTF8.GetBytes(messageBody)) {MessageId = Convert.ToString(Guid.NewGuid())};
                Log.Logger.Information("PublishAttestationMessage Send message to Service Bus");
                //Retry if failed to publish 

                await Retry.FixedIntervalAsync(
                    () => topicClient.SendAsync(message),
                    retryCount: retryCount, retryInterval: TimeSpan.FromSeconds(3),
                    retryingHandler: (sender, args) =>
                        Log.Logger.Error($@"PublishAttestationMessage RetryCount: {args.CurrentRetryCount} Exception: {args.LastException}"));
                messageId = message.MessageId;
                Log.Logger.Information("PublishAttestationMessage Send message with MessageId {messageId}", messageId);
            }
            catch (Exception ex)
            {
                Log.Logger.Debug(ex, "ValidateAttestationMessage exception");
                Log.Logger.Information("{Method} Publishing message failed");
                LogFailedMessage messageFail = new LogFailedMessage
                {
                    CAQHId = attestationMessage.CAQHId,
                    AttestationId = attestationMessage.AttestationId,
                    AttestationDateTimeStamp = attestationMessage.AttestationDateTimeStamp,
                    AttestationJsonId = attestationMessage.AttestationJsonId,
                    Error = ex.Message,
                    ErrorDetail = ex.StackTrace
                };

                await _messageLogService.LogFailedMessageAsync(messageFail);
                throw;
            }
            finally
            {
                // Close topic client connection
                topicClient?.CloseAsync();
            }

            return messageId;
        }

        /// <summary>
        /// Function to validate attestation message
        /// </summary>
        /// <param name="attestationMessage"></param>
        public void ValidateAttestationMessage(AttestationMessage attestationMessage)
        {
            
            var messageList = new List<string>();

			// CAQHId should not be null or empty
			if (string.IsNullOrWhiteSpace(attestationMessage.CAQHId))
			{
			    Log.Logger.Debug("ValidateAttestationMessage Error {errror}", ErrorMessages.NoCAQHId);

                messageList.Add(ErrorMessages.NoCAQHId);
			}

			// AttestationId should not be null or empty
			if (string.IsNullOrWhiteSpace(attestationMessage.AttestationId))
			{
			    Log.Logger.Debug("ValidateAttestationMessage Error {error}", ErrorMessages.NoAttestationId);

                messageList.Add(ErrorMessages.NoAttestationId);
			}

			// AttestationJsonId should not be null or empty
			if (string.IsNullOrWhiteSpace(attestationMessage.AttestationJsonId))
            {
                Log.Logger.Debug("ValidateAttestationMessage Error {errror}", ErrorMessages.NoAttestationJsonId);

                messageList.Add(ErrorMessages.NoAttestationJsonId);
            }

			// AttestationDateTimeStamp should not be null or empty
			if (string.IsNullOrWhiteSpace(attestationMessage.AttestationDateTimeStamp))
			{
			    Log.Logger.Debug("ValidateAttestationMessage Error {errror}", ErrorMessages.NoAttestationDateTimeStamp);

                messageList.Add(ErrorMessages.NoAttestationDateTimeStamp);
			}

			if (messageList.Count > 0)
                throw new PublisherException(string.Join("; ", messageList));

			else
			{
			    Log.Logger.Debug("ValidateAttestationMessage message in correct format");

            }
        }
    }


}
