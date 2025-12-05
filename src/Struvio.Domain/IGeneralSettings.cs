namespace Struvio.Domain;

/// <summary>
/// Genel uygulama ayarları
/// </summary>
public interface IGeneralSettings
{
    /// <summary>
    /// Uygulama adı
    /// </summary>
    string ApplicationName { get; set; }

    /// <summary>
    /// Uygulama web adresi
    /// </summary>
    string ApplicationUrl { get; set; }

    /// <summary>
    /// Api web adresi
    /// </summary>
    string ApplicationApiUrl { get; set; }

    /// <summary>
    /// Sayfada gösterilecek kayıt sayısı
    /// </summary>
    int DefaultPageSize { get; set; }

    /// <summary>
    /// Sayfada gösterilecek kayıt sayısı listesi
    /// </summary>
    string PageSizeList { get; set; }

    /// <summary>
    /// E-posta için SMTP sunucu adresi
    /// </summary>
    string SmtpHost { get; set; }

    /// <summary>
    /// SMTP port numarası
    /// </summary>
    int SmtpPort { get; set; }

    /// <summary>
    /// SMTP kullanıcı adı
    /// </summary>
    string SmtpUserName { get; set; }

    /// <summary>
    /// SMTP parolası
    /// </summary>
    string SmtpPassword { get; set; }

    /// <summary>
    /// SMTP güvenlik (SSL, TLS) seçenekleri
    /// </summary>
    string SmtpSecureSocketOptions { get; set; }

    /// <summary>
    /// SMTP gönderici e-posta adresi
    /// </summary>
    string SmtpFromAddress { get; set; }

    /// <summary>
    /// SMTP gönderici adı
    /// </summary>
    string SmtpFromDisplayName { get; set; }

    /// <summary>
    /// E-posta gövdesinde HTML kullanılabilir mi?
    /// </summary>
    bool SmtpIsBodyHtml { get; set; }

    /// <summary>
    /// SMTP için varsayılan kimlik bilgileri kullanılacak mı?
    /// </summary>
    bool SmtpUseDefaultCredentials { get; set; }

    /// <summary>
    /// SMTP için varsayılan ağ kimlik bilgileri kullanılacak mı?
    /// </summary>  
    bool SmtpUseDefaultNetworkCredentials { get; set; }

    /// <summary>
    /// E-postalar için şablon dosyalarının yolu
    /// </summary>
    string EmailTemplatePath { get; set; }

    /// <summary>
    /// Oturum Zaman Aşımı Saniye Cinsinden
    /// </summary>
    int SessionTimeoutInSeconds { get; set; }
}
