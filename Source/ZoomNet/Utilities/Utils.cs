using Microsoft.IO;
using System;
using System.Text.Encodings.Web;

namespace ZoomNet.Utilities
{
	/// <summary>
	/// Utils.
	/// </summary>
	internal static class Utils
	{
		public static RecyclableMemoryStreamManager MemoryStreamManager { get; } = new RecyclableMemoryStreamManager();

		public static void ValidateRecordPerPage(int recordsPerPage, int min = 1, int max = 300)
		{
			if (recordsPerPage < min || recordsPerPage > max)
			{
				throw new ArgumentOutOfRangeException(nameof(recordsPerPage), $"Records per page must be between {min} and {max}");
			}
		}

		public static string DoubleEncode(string value) => UrlEncoder.Default.Encode(UrlEncoder.Default.Encode(value));
	}
}
