using System.Net;

namespace FMCP.Tests;

public sealed class TestHttpMessageHandler : HttpMessageHandler
{
	private readonly Dictionary<string, HttpResponseMessage> _responses = [];

	public void SetupResponse(string url, HttpResponseMessage response)
	{
		_responses[url] = response;
	}

	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		if (_responses.TryGetValue(request.RequestUri.ToString(), out var response))
		{
			return Task.FromResult(response);
		}

		return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
	}
}