using ZoomNet.Resources;

namespace ZoomNet
{
	/// <summary>
	/// Interface for the Zoom REST client.
	/// </summary>
	public interface IZoomOAuthClient
	{
		/// <summary>
		/// Gets the resource that allows you to manage tokens.
		/// </summary>
		IToken Tokens { get; }
	}
}
