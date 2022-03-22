using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace Portal.Infrastructure
{
    public class CustomLoggingHttpMessageHandler : DelegatingHandler
    {
        private readonly ILogger<CustomLoggingHttpMessageHandler> _logger;
        public CustomLoggingHttpMessageHandler(ILogger<CustomLoggingHttpMessageHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"RequestExternalSystem ");
            sb.Append($"RequestTime:{DateTime.Now.ToString()} ");
            sb.Append($"Method:{request.Method} ");
            sb.Append($"RequestUri:{request.RequestUri} ");
            if (request.Content != null)
            {
                string requestBody = await request.Content.ReadAsStringAsync();
                sb.Append($"RequestBody:{requestBody} ");
            }
            if (request.Options.Any())
            {
                sb.Append($"Options:{JsonConvert.SerializeObject(request.Options, Formatting.None)} ");
            }

            var stopWatch = Stopwatch.StartNew();

            var httpResponseMessage = await base.SendAsync(request, cancellationToken);

            sb.Append($"StatusCode:{(int)httpResponseMessage.StatusCode} ");
            sb.Append($"{stopWatch.ElapsedMilliseconds}ms ");

            var reponseBody = await httpResponseMessage.Content.ReadAsStringAsync();
            sb.Append($"ReponseBody:{reponseBody} ");

            _logger.LogInformation(sb.ToString());

            return httpResponseMessage;
        }
    }
}
