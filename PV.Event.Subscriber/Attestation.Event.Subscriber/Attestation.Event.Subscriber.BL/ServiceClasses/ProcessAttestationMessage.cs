using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Attestation.Event.Subscriber.BL.ServiceContracts;
using Attestation.Event.Subscriber.DAL.Entities;
using Attestation.Event.Subscriber.DAL.RepositoryClasses;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;

namespace Attestation.Event.Subscriber.BL.ServiceClasses
{
    /// <summary>
    /// Handle the subscription of Azure service bus
    /// </summary>
    public class ProcessAttestationMessage
    {

        public static ISubscriptionClient SubscriptionClient;

        public static IConfiguration Config;



        /// <summary>
        /// Configure the message handler
        /// </summary>
        public static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 10,
                AutoComplete = false
            };

            SubscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }


        /// <summary>
        /// Process the messages
        /// </summary>
        /// <param name="message"></param>
        /// <param name="token"></param>
        /// <returns>Task</returns>
        static async Task ProcessMessagesAsync(Microsoft.Azure.ServiceBus.Message message, CancellationToken token)
        {
            Log.Information("Started Receiving New Message");

            AttestationMessage serviceMsg =
                JsonConvert.DeserializeObject<AttestationMessage>(Encoding.UTF8.GetString(message.Body));
            serviceMsg.SequenceNumber = message.SystemProperties.SequenceNumber;
            serviceMsg.CreatedDate = DateTime.Now;

            Log.Logger.Information("Message Received with following Information ");
            Log.Logger.Information("SequenceNumber : {SequenceNumber}, CAQHId : {caqhId} AttestationId : {AttestationId}, JSONId : {JsonId}, DateTimeStampOfMessage : {Stamp} ", serviceMsg.SequenceNumber, serviceMsg.CaqhId, serviceMsg.AttestationId, serviceMsg.AttestationJsonId, serviceMsg.AttestationDateTimeStamp);

            try
            {
                Log.Information("Message saving to DB Started ");
                // Save message into database
                IAttestationMessageProcessingService messageService = new AttestationMessageProcessingService(new MessageLogRepository<AttestationMessage>(Config));
               // await messageService.Add(serviceMsg);
                await Task.CompletedTask;
                Log.Logger.Information("Message saved to DB Successfully ");
            }
            catch (Exception exception)
            {
                Log.Logger.Error("Message saving to DB Failed ");
                Log.Logger.Error("Retry Message with Message Sequence Id {sequenceNumber} with retry count {Count} due to {Error}", message.SystemProperties.SequenceNumber, message.SystemProperties.DeliveryCount, exception.Message);
                await SubscriptionClient.AbandonAsync(message.SystemProperties.LockToken);
                return;
            }
            Log.Logger.Information("Ended Receiving Message");

            await SubscriptionClient.CompleteAsync(message.SystemProperties.LockToken);

            Log.Logger.Information("completed Receiving Message");

        }

        /// <summary>
        /// Use this handler to examine the exceptions received on the message pump.
        /// </summary>
        /// <param name="exceptionReceivedEventArgs">Exception received event arguments</param>
        /// <returns>Task</returns>
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            Log.Error(exceptionReceivedEventArgs.Exception, exceptionReceivedEventArgs.Exception.Message);

            return Task.CompletedTask;
        }
    }
}
