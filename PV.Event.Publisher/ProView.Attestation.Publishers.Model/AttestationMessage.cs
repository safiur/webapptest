namespace ProView.Attestation.Publisher.Model
{
    /// <summary>
    /// Attestation Message Model Class
    /// </summary>
    public class AttestationMessage
    {
		/// <summary>
		/// CAQH Id
		/// </summary>
		public string CAQHId { get; set; }

		/// <summary>
		/// Attestation Id
		/// </summary>
		public string AttestationId { get; set; }

		/// <summary>
		/// Attestation Json Id
		/// </summary>
		public string AttestationJsonId { get; set; }

		/// <summary>
		///Attestation Datetime Stamp
		/// </summary>
		public string AttestationDateTimeStamp { get; set; }
	}
}
