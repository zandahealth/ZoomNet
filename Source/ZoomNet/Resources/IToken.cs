using System.Threading;
using System.Threading.Tasks;
using ZoomNet.Models;

namespace ZoomNet.Resources
{
	/// <summary>
	/// Allows you to manage tokens.
	/// </summary>
	public interface IToken
	{
		/// <summary>
		/// To revoke a token.
		/// </summary>
		/// <param name="cancellationToken">Cancellation token.</param>
		/// <returns>TokenRevocation object.</returns>
		Task<TokenRevocation> RevokeAsync(CancellationToken cancellationToken = default);
	}
}
