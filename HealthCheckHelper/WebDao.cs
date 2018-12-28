using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheckHelper
{
    public abstract class WebDao : IHealthCheck
    {
        protected string HealthCheckUri { get; set; }
        protected string BaseUri { get; set; }
        protected HttpClient Client { get; set; }

        public WebDao(string baseUri, string healthCheckUri)
        {
            BaseUri = baseUri;
            HealthCheckUri = healthCheckUri;
            Client = new HttpClient()
            {
                BaseAddress = new Uri(BaseUri)
            };
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await Client.GetAsync(HealthCheckUri, cancellationToken);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return new HealthCheckResult(HealthStatus.Healthy);
            }

            return new HealthCheckResult(HealthStatus.Unhealthy);
        }
    }
}
