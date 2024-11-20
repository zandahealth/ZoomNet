using Pathoschild.Http.Client;
using Pathoschild.Http.Client.Extensibility;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using ZoomNet.Models;

namespace ZoomNet.Utilities
{
	internal class OAuthTokenHandlerForRevocation : IHttpFilter, ITokenHandler
	{
		public string Token
		{
			get
			{
				RefreshTokenIfNecessary(false);
				return _connectionInfo.AccessToken;
			}
		}

		public IConnectionInfo ConnectionInfo
		{
			get => _connectionInfo;
		}

		private static readonly ReaderWriterLockSlim _lock = new();

		private readonly OAuthTokenRevocationConnectionInfo _connectionInfo;
		private readonly HttpClient _httpClient;

		public OAuthTokenHandlerForRevocation(OAuthTokenRevocationConnectionInfo connectionInfo, HttpClient httpClient)
		{
			_connectionInfo = connectionInfo;
			_httpClient = httpClient;
		}

		public void OnRequest(IRequest request)
		{
			request.WithBasicAuthentication(_connectionInfo.ClientId, _connectionInfo.ClientSecret);
			request.Message.RequestUri = new Uri(request.Message.RequestUri.AbsoluteUri + $"?token={Token}");
		}

		public void OnResponse(IResponse response, bool httpErrorAsException) { }

		public string RefreshTokenIfNecessary(bool forceRefresh)
		{
			try
			{
				_lock.EnterUpgradeableReadLock();

				try
				{
					_lock.EnterWriteLock();

					var contentValues = new Dictionary<string, string>()
					{
						{ "grant_type", OAuthGrantType.RefreshToken.ToEnumString() },
						{ "refresh_token", _connectionInfo.RefreshToken }
					};

					var request = new HttpRequestMessage(HttpMethod.Post, "https://api.zoom.us/oauth/token");
					request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_connectionInfo.ClientId}:{_connectionInfo.ClientSecret}")));
					request.Content = new FormUrlEncodedContent(contentValues);
					var response = _httpClient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
					var responseContent = response.Content.ReadAsStringAsync(null).ConfigureAwait(false).GetAwaiter().GetResult();

					if (string.IsNullOrEmpty(responseContent)) throw new Exception(response.ReasonPhrase);

					var jsonResponse = JsonDocument.Parse(responseContent).RootElement;

					if (!response.IsSuccessStatusCode)
					{
						var reason = jsonResponse.GetPropertyValue("reason", "The Zoom API did not provide a reason");
						throw new ZoomException(reason, response, "No diagnostic available", null);
					}

					_connectionInfo.RefreshToken = jsonResponse.GetPropertyValue("refresh_token", string.Empty);
					_connectionInfo.AccessToken = jsonResponse.GetPropertyValue("access_token", string.Empty);
				}
				finally
				{
					if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
				}
			}
			finally
			{
				if (_lock.IsUpgradeableReadLockHeld) _lock.ExitUpgradeableReadLock();
			}

			return _connectionInfo.AccessToken;
		}
	}
}
