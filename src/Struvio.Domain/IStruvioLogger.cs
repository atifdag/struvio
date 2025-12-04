namespace Struvio.Domain;
public interface IStruvioLogger
{
    void Critical(Exception? exception, string? message, params object?[] args);
    void Critical(string? message, params object?[] args);
    void Error(Exception? exception, string? message, params object?[] args);
    void Error(string? message, params object?[] args);
    void Warning(string? message, params object?[] args);
    void Information(string? message, params object?[] args);
}
