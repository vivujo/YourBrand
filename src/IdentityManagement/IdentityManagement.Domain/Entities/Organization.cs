using YourBrand.Domain;
using YourBrand.IdentityManagement.Domain.Common;
using YourBrand.IdentityManagement.Domain.Events;
using YourBrand.Tenancy;

namespace YourBrand.IdentityManagement.Domain.Entities;

public class Organization : AuditableEntity, IHasTenant
{
    private readonly HashSet<User> _users = new HashSet<User>();
    private readonly HashSet<OrganizationUser> _organizationUsers = new HashSet<OrganizationUser>();

    private Organization() { }

    public Organization(OrganizationId id, string name, string? friendlyName)
    {
        Id = id;
        Name = name;
        FriendlyName = friendlyName;

        AddDomainEvent(new OrganizationCreated(Id));
    }

    public Organization(string name, string? friendlyName) : this(Guid.NewGuid().ToString(), name, friendlyName)
    {

    }

    public OrganizationId Id { get; private set; }

    public string Name { get; private set; }

    public void ChangeName(string name)
    {
        if (Name != name)
        {
            Name = name;
        }
    }

    public string? FriendlyName { get; private set; }

    public Tenant Tenant { get; set; }

    public TenantId TenantId { get; set; }

    public IReadOnlyCollection<User> Users => _users;

    public void AddUser(User user)
    {
        _organizationUsers.Add(new OrganizationUser
        {
            Tenant = Tenant,
            TenantId = Tenant.Id,
            Organization = this,
            OrganizationId = this.Id,
            User = user,
            UserId = user.Id
        });
    }

    public IReadOnlyCollection<OrganizationUser> OrganizationUsers => _organizationUsers;
}