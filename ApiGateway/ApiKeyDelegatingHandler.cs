namespace ApiGateway
{
	public class ApiKeyDelegatingHandler : DelegatingHandler
	{
		private readonly IConfiguration _config;

		public ApiKeyDelegatingHandler(IConfiguration config)
		{
			_config = config;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var apiKey = _config.GetValue<string>("ApiKey");
			request.Headers.Add("x-api-key", apiKey);
			return await base.SendAsync(request, cancellationToken);
		}
	}
}
