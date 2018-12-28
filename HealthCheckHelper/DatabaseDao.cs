using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheckHelper
{
    public abstract class DatabaseDao : IHealthCheck
    {
        protected readonly string ConnectionString;

        protected DatabaseDao(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    await connection.OpenAsync(cancellationToken);
                    connection.Close();
                }
                catch (Exception ex)
                {
                    return new HealthCheckResult(HealthStatus.Unhealthy, ex.Message);
                }
            }

            return new HealthCheckResult(HealthStatus.Healthy, "Sucessful connection to DB established");
        }
    }
}