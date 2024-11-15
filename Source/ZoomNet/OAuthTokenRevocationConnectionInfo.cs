using System;

namespace ZoomNet
{
	/// <summary>
	/// Connect using OAuth to revoke a token.
	/// </summary>
	public class OAuthTokenRevocationConnectionInfo : IConnectionInfo
	{
		/// <summary>
		/// Gets the client id.
		/// </summary>
		public string ClientId { get; private set; }

		/// <summary>
		/// Gets the client secret.
		/// </summary>
		public string ClientSecret { get; private set; }

		/// <summary>
		/// Gets the refresh token.
		/// </summary>
		public string RefreshToken { get; internal set; }

		/// <summary>
		/// Gets the refresh token.
		/// </summary>
		public string AccessToken { get; internal set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="OAuthTokenRevocationConnectionInfo"/> class.
		/// </summary>
		/// <param name="clientId">Your Client Id.</param>
		/// <param name="clientSecret">Your Client Secret.</param>
		/// <param name="refreshToken">The refresh token.</param>
		/// <returns>The connection info.</returns>
		public static OAuthTokenRevocationConnectionInfo WithRefreshToken(string clientId, string clientSecret, string refreshToken)
		{
			if (string.IsNullOrEmpty(clientId)) throw new ArgumentNullException(nameof(clientId));
			if (string.IsNullOrEmpty(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));
			if (string.IsNullOrEmpty(refreshToken)) throw new ArgumentNullException(nameof(refreshToken));

			return new OAuthTokenRevocationConnectionInfo
			{
				ClientId = clientId,
				ClientSecret = clientSecret,
				RefreshToken = refreshToken
			};
		}
	}
}
