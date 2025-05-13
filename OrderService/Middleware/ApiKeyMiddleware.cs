namespace OrderService.Middleware
{
	public class ApiKeyMiddleware
	{
		private readonly RequestDelegate _next;
		private const string APIKEYNAME = "x-api-key";
		private readonly IConfiguration _configuration;

		public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
		{
			_next = next;
			_configuration = configuration;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
			{
				context.Response.StatusCode = 401;
				await context.Response.WriteAsync("API Key was not provided.");
				return;
			}

			var apiKey = _configuration.GetValue<string>("ApiKey");

			if (!apiKey.Equals(extractedApiKey))
			{
				context.Response.StatusCode = 401;
				await context.Response.WriteAsync("Unauthorized client.");
				return;
			}

			await _next(context);
		}
	}

}
