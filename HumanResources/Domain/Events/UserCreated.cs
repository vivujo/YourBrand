using YourBrand.HumanResources.Domain.Common;

namespace YourBrand.HumanResources.Domain.Events;

public class UserCreated : DomainEvent
{
    public string UserId { get; set; }

    public UserCreated(string userId)
    {
        UserId = userId;
    }
}
