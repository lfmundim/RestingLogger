using RestEase;
using RestingLogger.Services;
using Serilog;
using System;
using System.Net.Http;

namespace RestingLogger.Builders
{
    /// <summary>
    /// Class responsible for building the Log-enabled HttpClient
    /// </summary>
    public class LoggedRestClientBuilder
    {
        /// <summary>
        /// Builds a new HttpClient for the type T using the given URL and Serilog.ILogger
        /// </summary>
        /// <typeparam name="T">RestEase Interface for the HttpClient</typeparam>
        /// <param name="baseUrl">Base URL used on the requests</param>
        /// <param name="logger">Serilog.ILogger instance</param>
        /// <returns>Log-enabled RestEase HttpClient for the given URL</returns>
        public virtual T BuildLoggedClient<T>(string baseUrl, ILogger logger)
        {
            var httpClient = new HttpClient(new RestLoggingHandler(logger))
            {
                BaseAddress = new Uri(baseUrl),
                
            };

            return RestClient.For<T>(httpClient);
        }
    }
}
