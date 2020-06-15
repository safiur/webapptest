namespace ProView.Attestation.Publisher.Model
{
    /// <summary>
    /// Attestation failed message
    /// </summary>
    public class LogFailedMessage : AttestationMessage
    {
        /// <summary>
        /// Attestation error
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Attestation error details
        /// </summary>
        public string ErrorDetail { get; set; }
    }
}
