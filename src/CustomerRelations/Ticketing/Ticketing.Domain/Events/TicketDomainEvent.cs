using System.Text.Json.Serialization;
using YourBrand.Ticketing.Domain.ValueObjects;
using OrganizationId = YourBrand.Domain.OrganizationId;


namespace YourBrand.Ticketing.Domain.Events;

[JsonDerivedType(typeof(TicketAssigneeUpdated), typeDiscriminator: nameof(TicketAssigneeUpdated))]
[JsonDerivedType(typeof(TicketCreated), typeDiscriminator: nameof(TicketCreated))]
[JsonDerivedType(typeof(TicketDeleted), typeDiscriminator: nameof(TicketDeleted))]
[JsonDerivedType(typeof(TicketDescriptionUpdated), typeDiscriminator: nameof(TicketDescriptionUpdated))]
[JsonDerivedType(typeof(TicketEstimatedHoursUpdated), typeDiscriminator: nameof(TicketEstimatedHoursUpdated))]
[JsonDerivedType(typeof(TicketRemainingHoursUpdated), typeDiscriminator: nameof(TicketRemainingHoursUpdated))]
[JsonDerivedType(typeof(TicketStatusUpdated), typeDiscriminator: nameof(TicketStatusUpdated))]
[JsonDerivedType(typeof(TicketSubjectUpdated), typeDiscriminator: nameof(TicketSubjectUpdated))]
public record TicketDomainEvent(OrganizationId OrganizationId, TicketId TicketId) : DomainEvent, IHasOrganization2, IHasTicket;

public interface IHasTicket
{
    TicketId TicketId { get; }
}

public interface IHasOrganization2
{
    OrganizationId OrganizationId { get; }
}