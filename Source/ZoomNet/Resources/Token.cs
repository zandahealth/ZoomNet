using Pathoschild.Http.Client;
using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Token resource.
	/// </summary>
	public class Token : IToken
	{
		private readonly Pathoschild.Http.Client.IClient _client;

		internal Token(Pathoschild.Http.Client.IClient client)
		{
			_client = client;
		}

		/// <summary>
		/// To revoke a token.
		/// </summary>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>TokenRevocation object.</returns>
		public Task<TokenRevocation> RevokeAsync(CancellationToken cancellationToken = default)
		{
			return _client
			.PostAsync($"https://zoom.us/oauth/revoke")
			.WithCancellationToken(cancellationToken)
				.AsObject<TokenRevocation>();
		}
	}
}
