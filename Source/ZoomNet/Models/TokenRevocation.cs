using System.Text.Json.Serialization;

namespace ZoomNet.Models
{
	/// <summary>
	/// To revoke a token.
	/// </summary>
	public class TokenRevocation
	{
		/// <summary>
		/// Gets or sets status of the token revocation.
		/// </summary>
		[JsonPropertyName("status")]
		public string Status { get; set; }

		/// <summary>
		/// Gets or sets reason of the error during token revocation error.
		/// </summary>
		[JsonPropertyName("reason")]
		public string Reason { get; set; }

		/// <summary>
		/// Gets or sets the generic error error message during token revocation error.
		/// </summary>
		[JsonPropertyName("error")]
		public string Error { get; set; }
	}
}
