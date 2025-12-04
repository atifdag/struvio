namespace Struvio.Domain.Enums;
public enum LogoutType
{
    // Kullanıcı kendi oturum isteyerek sonlandırdığında
    Normal = 1,

    // Sistemsel bir neden ya da yönetici tarafından oturum sonlandıysa
    Forced = 2,

    // Oturum süresi dolduğunda
    TimeOut = 3,
}