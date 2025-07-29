namespace FMCP.Tests;

public sealed class TestHttpClientFactory : IHttpClientFactory
{
	private readonly HttpClient _client;

	public TestHttpClientFactory(HttpClient client)
	{
		_client = client;
	}

	public HttpClient CreateClient(string name) => _client;
}