namespace Accsivo.Common.Enums;

/// <summary>
/// Hata kodları
/// </summary>
public enum ErrorCodes
{
    // Kritik hata
    FatalError = 1000,
    // Hesap daha önce etkinleştirilmiş
    AccountHasBeenPreviouslyActivated = 1001,
    // İlgili kayıt silinemedi
    AssociatedRecordNotDeleted = 1002,
    // Veritabanı bağlantısı geçersiz
    DatabaseConnectionInvalid = 1003,
    // Mevcut veritabanı kaldırılmadı
    ExistingDatabaseNotRemoved = 1004,
    // Alan tekrar ediyor
    FieldDuplicated = 1005,
    // Alan geçersiz
    FieldInvalid = 1006,
    // Alan boş
    FieldIsEmpty = 1007,
    // Alan uzunluk sınırı aşıldı
    FieldLengthLimit = 1008,
    // Kimlik kullanıcısı bulunamadı
    IdentityUserNotFound = 1009,
    // Eski şifre yanlış
    IncorrectOldPassword = 1010,
    // Şifre yanlış
    IncorrectPassword = 1011,
    // Geçersiz API istemcisi
    InvalidApiClient = 1012,
    // Geçersiz kod
    InvalidCode = 1013,
    // Geçersiz varlık
    InvalidEntitiy = 1014,
    // Geçersiz dosya türü
    InvalidFileType = 1015,
    // Geçersiz lisans anahtarı
    InvalidLicenseKey = 1016,
    // Geçersiz şifre
    InvalidPassword = 1017,
    // Öğe onaylanmadı
    ItemNotApproved = 1018,
    // LDAP bağlantısı geçersiz
    LdapConnectionInvalid = 1019,
    // Yeni veritabanı oluşturulamadı
    NewDatabaseNotCreated = 1020,
    // İzin yok
    NoPermission = 1021,
    // Organizasyon limiti aşıldı
    OrganizationLimit = 1022,
    // Üst kayıt bulunamadı
    ParentNotFound = 1023,
    // Şifreler eşleşmiyor
    PasswordsDoNotMatch = 1024,
    // Kayıt bulunamadı
    RecordNotFound = 1025,
    // Sayfada kayıt bulunamadı
    RecordNotFoundInPage = 1026,
    // Zaman aşımına uğradı
    TimedOut = 1027,
    // Kullanıcının rolü yok
    UserHasNoRole = 1028,
    // Kullanıcı bulunamadı
    UserNotFound = 1029,
    // Kullanıcı kaynak türü seçenekleri ayarlanmamış
    UserSourceTypeOptionsNotSet = 1030,
    // PSS imzası için konu adı yok
    VerifyPssSignatureNoSubjectName = 1031,
    // PSS imzası için sertifika geçersiz
    VerifyPssSignatureCertificateInvalid = 1032,
    // PSS imzası geçersiz
    VerifyPssSignatureSignatureInvalid = 1033,
    // E-imza sonlandırma imzası geçersiz
    FinalizeESignatureSignatureInvalid = 1034,
    // E-imza sonlandırma sertifikası geçersiz
    FinalizeESignatureCertificateInvalid = 1035,
    // Kayıt zaten mevcut
    RecordAlreadyExists = 1036,
    // Geçersiz işlem
    InvalidOperation = 1037,
    // Bilinmeyen hata
    Unknown = 1038,
    // Bir şeyler yanlış gitti
    SomethingWentWrong = 1039,
    // Erişim reddedildi
    AccessDenied = 1040
}
