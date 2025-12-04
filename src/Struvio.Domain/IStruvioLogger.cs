namespace Struvio.Domain;

/// <summary>
/// Struvio uygulaması için loglama arayüzü. Farklı seviyelerde log kaydı sağlar.
/// </summary>
public interface IStruvioLogger
{
    /// <summary>
    /// Kritik seviyede log kaydı yapar (hata ile birlikte).
    /// </summary>
    /// <param name="exception">Oluşan istisna</param>
    /// <param name="message">Log mesajı</param>
    /// <param name="args">Mesaj parametreleri</param>
    void Critical(Exception? exception, string? message, params object?[] args);
    
    /// <summary>
    /// Kritik seviyede log kaydı yapar.
    /// </summary>
    /// <param name="message">Log mesajı</param>
    /// <param name="args">Mesaj parametreleri</param>
    void Critical(string? message, params object?[] args);
    
    /// <summary>
    /// Hata seviyesinde log kaydı yapar (hata ile birlikte).
    /// </summary>
    /// <param name="exception">Oluşan istisna</param>
    /// <param name="message">Log mesajı</param>
    /// <param name="args">Mesaj parametreleri</param>
    void Error(Exception? exception, string? message, params object?[] args);
    
    /// <summary>
    /// Hata seviyesinde log kaydı yapar.
    /// </summary>
    /// <param name="message">Log mesajı</param>
    /// <param name="args">Mesaj parametreleri</param>
    void Error(string? message, params object?[] args);
    
    /// <summary>
    /// Uyarı seviyesinde log kaydı yapar.
    /// </summary>
    /// <param name="message">Log mesajı</param>
    /// <param name="args">Mesaj parametreleri</param>
    void Warning(string? message, params object?[] args);
    
    /// <summary>
    /// Bilgi seviyesinde log kaydı yapar.
    /// </summary>
    /// <param name="message">Log mesajı</param>
    /// <param name="args">Mesaj parametreleri</param>
    void Information(string? message, params object?[] args);
}
