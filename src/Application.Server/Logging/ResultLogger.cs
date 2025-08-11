using FluentResults;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Application.Server.Logging;

public class ResultLogger(ILogger<ResultLogger> logger) : IResultLogger
{
    public void Log(string context, string content, ResultBase result, LogLevel logLevel)
    {
        if (!result.IsFailed)
        {
            logger.Log(logLevel, "[{Context}] {Content} - Success", context, content);
            return;
        }

        foreach (var error in result.Errors)
        {
            logger.Log(logLevel,
                "[{Context}] {Content} - Error: {Message}. Reasons: {Reasons}. Metadata: {Metadata}",
                context,
                content,
                error.Message,
                string.Join("; ", error.Reasons.Select(r => r.Message)),
                (error as Error)?.Metadata
            );
        }
    }

    public void Log<TContext>(string content, ResultBase result, LogLevel logLevel)
    {
        if (!result.IsFailed)
        {
            logger.Log(logLevel, "[{Context}] {Content} - Success", typeof(TContext).Name, content);
            return;
        }

        foreach (var error in result.Errors)
        {
            logger.Log(logLevel,
                "[{Context}] {Content} - Error: {Message}. Reasons: {Reasons}. Metadata: {Metadata}",
                typeof(TContext).Name,
                content,
                error.Message,
                string.Join("; ", error.Reasons.Select(r => r.Message)),
                (error as Error)?.Metadata
            );
        }
    }
}
