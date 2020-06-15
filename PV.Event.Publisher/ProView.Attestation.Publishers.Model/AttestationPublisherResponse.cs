namespace ProView.Attestation.Publisher.Model
{
    /// <summary>
    /// Attestation Publisher Response Model class
    /// </summary>
    public class AttestationPublisherResponse
    {
        /// <summary>
        /// Standard Http status code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Text status message :SUCCESS||BAD REQUEST
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Http response body
        /// </summary>
        public object Data { get; set; } = string.Empty;

        /// <summary>
        /// Publisher Error
        /// </summary>
        public string Error { get; set; } = string.Empty;

        /// <summary>
        /// Publisher message
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
