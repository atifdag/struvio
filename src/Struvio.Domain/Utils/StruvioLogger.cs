namespace Struvio.Domain.Utils;

public class StruvioLogger(ILogger<StruvioLogger> logger) : IStruvioLogger
{
    public void Critical(Exception? exception, string? message, params object?[] args)
    {
        logger.LogCritical(exception, message, args);
    }

    public void Critical(string? message, params object?[] args)
    {
        logger.LogCritical(message, args);
    }

    public void Error(Exception? exception, string? message, params object?[] args)
    {
        logger.LogError(exception, message, args);
    }

    public void Error(string? message, params object?[] args)
    {
        logger.LogError(message, args);
    }

    public void Information(string? message, params object?[] args)
    {
        logger.LogInformation(message, args);
    }

    public void Warning(string? message, params object?[] args)
    {
        logger.LogWarning(message, args);
    }
}
