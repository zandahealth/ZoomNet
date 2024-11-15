using Pathoschild.Http.Client;
using Pathoschild.Http.Client.Extensibility;
using System;
using System.Net.Http;
using ZoomNet.Json;
using ZoomNet.Resources;
using ZoomNet.Utilities;

namespace ZoomNet
{
	/// <summary>
	/// REST client for interacting with Zoom's OAUTH endpoints.
	/// </summary>
	public class ZoomOAuthClient : IZoomOAuthClient, IDisposable
	{
		private const string ZOOM_OAUTH_BASE_URI = "https://zoom.us/oauth";

		private HttpClient _httpClient;
		private Pathoschild.Http.Client.IClient _fluentClient;

		/// <summary>
		/// Gets the resource that allows you to manage tokens.
		/// </summary>
		public IToken Tokens { get; private set; }

		#region CTOR

		/// <summary>
		/// Initializes a new instance of the <see cref="ZoomOAuthClient"/> class.
		/// </summary>
		/// <param name="connectionInfo">Connection information.</param>
		/// <exception cref="ArgumentNullException">ArgumentNullException.</exception>
		/// <exception cref="ZoomException">ZoomException.</exception>
		public ZoomOAuthClient(IConnectionInfo connectionInfo)
		{
			if (connectionInfo == null) throw new ArgumentNullException(nameof(connectionInfo));

			_httpClient = new HttpClient();

			_fluentClient = new FluentClient(new Uri(ZOOM_OAUTH_BASE_URI), _httpClient);

			_fluentClient.Filters.Remove<DefaultErrorFilter>();

			// Remove all the built-in formatters and replace them with our custom JSON formatter
			_fluentClient.Formatters.Clear();
			_fluentClient.Formatters.Add(new JsonFormatter());

			if (connectionInfo is OAuthTokenRevocationConnectionInfo oAuthTokenRevocationConnectionInfo)
			{
				var tokenHandler = new OAuthTokenHandlerForRevocation(oAuthTokenRevocationConnectionInfo, _httpClient);
				_fluentClient.Filters.Add(tokenHandler);
				_fluentClient.SetRequestCoordinator(new ZoomRetryCoordinator(new Http429RetryStrategy(), tokenHandler));
			}
			else
			{
				throw new ZoomException($"{connectionInfo.GetType()} is an unknown connection type", null, null, null, null);
			}

			_fluentClient.Filters.Add(new ZoomErrorHandler());

			Tokens = new Token(_fluentClient);
		}

		/// <summary>
		/// Finalizes an instance of the <see cref="ZoomOAuthClient"/> class.
		/// </summary>
		~ZoomOAuthClient()
		{
			// The object went out of scope and finalized is called.
			// Call 'Dispose' to release unmanaged resources
			// Managed resources will be released when GC runs the next time.
			Dispose(false);
		}

		#endregion

		#region PUBLIC METHODS

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			// Call 'Dispose' to release resources
			Dispose(true);

			// Tell the GC that we have done the cleanup and there is nothing left for the Finalizer to do
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				ReleaseManagedResources();
			}
		}

		#endregion

		#region PRIVATE METHODS

		private void ReleaseManagedResources()
		{
			if (_fluentClient != null)
			{
				_fluentClient.Dispose();
				_fluentClient = null;
			}

			if (_httpClient != null)
			{
				_httpClient.Dispose();
				_httpClient = null;
			}
		}

		#endregion
	}
}
