using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions;

public class DatabaseWarmUpService(IConfiguration configuration, ILogger<DatabaseWarmUpService> logger)
{
    public async Task WarmUpDatabaseIfRequiredAsync()
    {
        var connectionString = configuration.GetConnectionString("DefaultSQL");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            logger.LogWarning("Database connection string is missing from environment variables.");
            return;
        }

        var customEnvironment = configuration.GetValue<string>("CustomEnvironment");
        if (customEnvironment is not ("Development" or "Staging"))
        {
            logger.LogInformation("Skipping database warm-up because custom environment value is set to {environmen}",
                customEnvironment);
            return;
        }

        const int maxRetries = 10;
        const int delayBetweenRetriesMs = 5000;

        for (var attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                await using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                logger.LogInformation("Database warm-up successful.");
                return;
            }
            catch (Exception ex)
            {
                logger.LogWarning($"Attempt {attempt} failed: {ex.Message}");

                if (attempt < maxRetries)
                {
                    logger.LogInformation($"Retrying in {delayBetweenRetriesMs / 1000} seconds...");
                    await Task.Delay(delayBetweenRetriesMs);
                }
                else
                {
                    logger.LogError("All retry attempts failed.");
                }
            }
        }
    }
}