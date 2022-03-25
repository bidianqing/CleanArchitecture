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

            sb.AppendLine($"RequestExternalSystem ");
            sb.AppendLine($"RequestTime:{DateTime.Now.ToString()} ");
            sb.AppendLine($"Method:{request.Method} ");
            sb.AppendLine($"RequestUri:{request.RequestUri} ");
            if (request.Content != null)
            {
                string requestBody = await request.Content.ReadAsStringAsync();
                sb.AppendLine($"RequestBody:{requestBody} ");
            }
            if (request.Options.Any())
            {
                sb.AppendLine($"Options:{JsonConvert.SerializeObject(request.Options, Formatting.None)} ");
            }

            var stopWatch = Stopwatch.StartNew();

            var httpResponseMessage = await base.SendAsync(request, cancellationToken);

            sb.AppendLine($"StatusCode:{(int)httpResponseMessage.StatusCode} {stopWatch.ElapsedMilliseconds}ms");

            var reponseBody = await httpResponseMessage.Content.ReadAsStringAsync();
            sb.AppendLine($"ReponseBody:{reponseBody} ");

            _logger.LogInformation(sb.ToString());

            return httpResponseMessage;
        }
    }
}
