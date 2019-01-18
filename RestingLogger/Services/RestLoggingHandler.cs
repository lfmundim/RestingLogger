using Serilog;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RestingLogger.Services
{
    /// <summary>
    /// Class that overrides the SendAsync method to fully log RESTful requests and responses
    /// </summary>
    public class RestLoggingHandler : DelegatingHandler
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Class that overrides the SendAsync method to fully log RESTful requests and responses
        /// </summary>
        /// <param name="logger">Serilog.ILogger instance</param>
        public RestLoggingHandler(ILogger logger) : base(new HttpClientHandler())
        {
            _logger = logger;
        }

        /// <summary>
        /// Sends and logs a request, logging its response as well. 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var correlationId = Guid.NewGuid();
            string content = await GetContentAsync(request.Content);
            _logger.Information("Id: {@correlationId}\tRequest:{@request}, Content:{@content}", correlationId, request, content);

            try
            {
                var response = await base.SendAsync(request, cancellationToken);
                content = await GetContentAsync(response.Content);
                _logger.Information("Id: {@correlationId}\tResponse:{@response}, Content:{@content}", correlationId, response, content);
                return response;
            }
            catch (Exception ex)
            {
                _logger.Warning(ex, $"Failed to get response: {ex}");
                throw;
            }
        }

        private async Task<string> GetContentAsync(HttpContent content) => content == null ? "{\"value\": \"empty\"}" : await content.ReadAsStringAsync();
    }
}
