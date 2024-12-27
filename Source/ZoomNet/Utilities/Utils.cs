using Microsoft.IO;
using System.Text.Encodings.Web;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Utils.
	/// </summary>
	internal static class Utils
	{
		public static RecyclableMemoryStreamManager MemoryStreamManager { get; } = new RecyclableMemoryStreamManager();

		public static string DoubleEncode(string value) => UrlEncoder.Default.Encode(UrlEncoder.Default.Encode(value));
	}
}
