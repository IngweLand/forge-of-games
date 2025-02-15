using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Services.Hoh;

public class InGameDataProcessingServiceBase(ILogger<InGameDataProcessingServiceBase> logger)
{
    protected ILogger<InGameDataProcessingServiceBase> Logger { get; } = logger;

    protected byte[] DecodeInternal(string inputData)
    {
        byte[] data;
        try
        {
            data = Convert.FromBase64String(inputData);
        }
        catch (Exception ex)
        {
            const string msg = "Failed to decode Base64 string of in-game data";
            Logger.LogError(ex, msg);
            throw new InvalidOperationException(msg, ex);
        }

        if (data == null || data.Length == 0)
        {
            const string msg = "In-game data cannot be null or empty";
            Logger.LogError(msg);
            throw new ArgumentException(msg, nameof(data));
        }

        return data;
    }
}
