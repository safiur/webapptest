using System;
using Dapper.Contrib.Extensions;

namespace Attestation.Event.Subscriber.DAL.Entities
{
    /// <summary>
    /// Azure service bus message data structure
    /// </summary>
    [Table("AttestationMessage")]
    public class AttestationMessage
    {
        [Key]
        public long MessageId { get; set; }
        public long SequenceNumber { get; set; }
        public string CaqhId { get; set; }
        public string AttestationId { get; set; }
        public string AttestationJsonId { get; set; }
        public string AttestationDateTimeStamp { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
