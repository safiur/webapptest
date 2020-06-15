using System;
using Dapper.Contrib.Extensions;

namespace ProView.Attestation.Publisher.DAL.Entities
{
    /// <summary>
    /// Azure service bus message data structure
    /// </summary>
    [Table("AttestationPublisherFailedMessage")]
    public class FailedAttestationMessage
    {
        public string CaqhId { get; set; }
        public string AttestationId { get; set; }
        public string AttestationJsonId { get; set; }
        public string AttestationDateTimeStamp { get; set; }
        public string ErrorDetail { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ErrorMessage { get; set; }
    }
}
