using System;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using Serilog.Context;

namespace ProView.Attestation.Publisher.Controllers
{
    /// <summary>
    /// Attestation Message Controller
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AttestationMessageController : ControllerBase
    {
        /// <summary>
        /// Attestation Publisher service
        /// </summary>
        private readonly IAttestationPublisherService _attestationPublisherService;

        private readonly ILogger<AttestationMessageController> _logger;


        /// <summary>
        /// Attestation Message Controller constructor
        /// </summary>
        /// <param name="attestationPublisherService"></param>
        /// <param name="logger"></param>
        /// <param name="messageService"></param>
        public AttestationMessageController(
            IAttestationPublisherService attestationPublisherService,
            ILogger<AttestationMessageController> logger, IMessageLogService messageService)
        {
            _attestationPublisherService = attestationPublisherService;
            _logger = logger;
            
        }

        /// <summary>
        /// Action to publish attestation message
        /// </summary>
        /// <param name="attestationMessage">Attestation Message Object</param>
        /// <returns>Action result</returns>
        /// <response code="401">If call is UnAuthorized</response>
        /// <response code="200">If message is published successfully</response>
        /// <response code="400">If message request is invalid</response>
        /// <response code="500">If any internal server error occurred</response> 
        [HttpPost]
        [Route("publish")]
        public IActionResult Publish([FromBody] AttestationMessage attestationMessage)
        {
            using (LogContext.PushProperty("Method", "Publish"))
            {
                IActionResult response;
                Log.Logger.Information("{Method} Function started", "Publish");
                Log.Logger.Information("Publish Attestation Message content {@attestationMessage}", attestationMessage);

                try
                {
                    Log.Logger.Information("{Method} Calling Azure Service Bus to Publish Bus");
                    _attestationPublisherService.ValidateAttestationMessage(attestationMessage);
                    var messageId = _attestationPublisherService.PublishAttestationMessage(attestationMessage)
                        .GetAwaiter().GetResult();
                    ;

                    var data = JsonConvert.SerializeObject(new AttestationPublisherResponse
                    {
                        StatusCode = Convert.ToInt32(HttpStatusCode.OK),
                        Status = StatusConstants.SuccessText,
                        Data = new {MesageID = messageId},
                        Message = StatusConstants.PublisherSuccessMessage
                    });
                    response = Ok(data);
                    Log.Logger.Information("Publish Publishing to Bus Successful for {@attestationMessage}",
                        attestationMessage);

                }

                catch (PublisherException publisherException)
                {

                    var data = JsonConvert.SerializeObject(new AttestationPublisherResponse
                    {
                        StatusCode = Convert.ToInt32(HttpStatusCode.BadRequest),
                        Status = StatusConstants.BadRequestText,
                        Error = publisherException.Message
                    });

                    response = BadRequest(data);
                }

                catch (Exception exception)
                {
                    Log.Logger.Error(exception, "{Method} Publishing message failed");

                    var data = JsonConvert.SerializeObject(new AttestationPublisherResponse
                    {
                        StatusCode = Convert.ToInt32(HttpStatusCode.InternalServerError),
                        Error = exception.Message
                    });
                    response = BadRequest(data);
                }

                return response;
            }

        }

    }
}
