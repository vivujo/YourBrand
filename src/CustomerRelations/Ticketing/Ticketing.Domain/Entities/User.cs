using YourBrand.Ticketing.Domain.ValueObjects;
using YourBrand.Tenancy;
using YourBrand.Identity;

namespace YourBrand.Ticketing.Domain.Entities;

public class User : AggregateRoot<UserId>, IAuditable, IHasTenant
{
    public User(UserId id, string name, string email)
        : base(id)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    public string Name { get; set; }

    public TenantId TenantId { get; set; }

    public string Email { get; set; }

    public User? CreatedBy { get; set; }

    public UserId? CreatedById { get; set; }

    public DateTimeOffset Created { get; set; }

    public User? LastModifiedBy { get; set; }

    public UserId? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }

    public List<Organization> Organizations { get; set; }

    public List<OrganizationUser> OrganizationUsers { get; set; }
}