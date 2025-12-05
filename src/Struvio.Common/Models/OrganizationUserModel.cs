namespace Struvio.Common.Models;

public record OrganizationUserModel(Guid UserId, string? IdentityNumber, string? Firstname, string? Lastname, string? Email, string? PhoneNumber, IdCodeName[] Roles)
{
    public string? DisplayName => Firstname + " " + Lastname;
}
