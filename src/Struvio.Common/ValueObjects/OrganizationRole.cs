namespace Struvio.Common.ValueObjects;

/// <summary>
/// OrganizationRole yapısı, organizasyon kimliği, organizasyon adı, rol kimliği ve rol adı içerir.
/// </summary>
/// <param name="organizationId"></param>
/// <param name="organizationName"></param>
/// <param name="roleId"></param>
/// <param name="roleName"></param>
public struct OrganizationRole(Guid organizationId, string? organizationName, Guid roleId, string? roleName)
{
    // Organizasyonun benzersiz kimliği
    public Guid OrganizationId { get; set; } = organizationId;

    // Organizasyonun adı (isteğe bağlı)
    public string? OrganizationName { get; set; } = organizationName;

    // Rolün benzersiz kimliği
    public Guid RoleId { get; set; } = roleId;

    // Rolün adı (isteğe bağlı)
    public string? RoleName { get; set; } = roleName;
}
