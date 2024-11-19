using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet
{
	/// <summary>
	/// Interface for the Zoom REST client.
	/// </summary>
	public interface IZoomOAuthClient
	{
		/// <summary>
		/// To revoke a user's access token.
		/// </summary>
		/// <param name="cancellationToken">CancellationToken.</param>
		/// <returns>TokenRevocation object.</returns>
		Task<TokenRevocation> RevokeAsync(CancellationToken cancellationToken);
	}
}
