using YourBrand.ChatApp.Domain.ValueObjects;

namespace YourBrand.ChatApp.Domain.Entities;

public sealed class Channel : AggregateRoot<ChannelId>, IAuditable
{
    private Channel() : base(new ChannelId(Guid.NewGuid()))
    {
    }

    public Channel(string title)
        : base(new ChannelId(Guid.NewGuid()))
    {
        Title = title;

        // Todo: Emit Domain Event
    }

    internal Channel(ChannelId id, string title)
        : base(id)
    {
        Title = title;
    }

    public string Title { get; private set; } = null!;

    public bool Rename(string newTitle)
    {
        var oldTitle = Title;

        if (newTitle == Title)
            return false;

        Title = newTitle;

        // Todo: Emit Domain Event

        AddDomainEvent(new ChannelRenamed(Id, newTitle, oldTitle));
        return true;
    }

    public ChannelSettings Settings { get; set; } = new ChannelSettings();

    readonly HashSet<ChannelParticipant> _participants = new HashSet<ChannelParticipant>();

    public IReadOnlyCollection<ChannelParticipant> Participants => _participants;

    public Result AddParticipant(UserId userId)
    {
        var participant = Participants.FirstOrDefault(x => x.UserId == userId);

        if (participant is not null) return Result.Failure(Errors.Channels.NotParticipantInChannel);

        participant = new ChannelParticipant(userId, DateTimeOffset.UtcNow);

        _participants.Add(participant);

        AddDomainEvent(new ParticipantAddedToChannel(Id, participant.Id));

        return Result.Success;
    }

    public bool RemoveParticipant(UserId userId)
    {
        var participant = Participants.First(x => x.UserId == userId);

        if (participant is null) return false;

        participant.Left = DateTimeOffset.UtcNow;
        _participants.Remove(participant);

        AddDomainEvent(new ParticipantRemovedFromChannel(Id, participant.Id));

        return true;
    }

    public UserId? CreatedById { get; set; } = null!;
    public DateTimeOffset Created { get; set; }

    public UserId? LastModifiedById { get; set; }
    public DateTimeOffset? LastModified { get; set; }
}

public class ChannelSettings
{
    public bool? IsReadOnly { get; set; }

    public bool? DisallowDisplayNames { get; set; }

    public bool? SoftDeleteMessages { get; set; }
}

public sealed class ChannelParticipant : Entity<ChannelParticipantId>
{
    private ChannelParticipant() : base(new ChannelParticipantId())
    {
    }

    public ChannelParticipant(UserId userId, DateTimeOffset joined)
        : base(new ChannelParticipantId())
    {

        UserId = userId;
        Joined = joined;
    }

    public ChannelId ChannelId { get; private set; }

    public UserId UserId { get; set; }

    public string? DisplayName { get; set; }

    public DateTimeOffset Joined { get; set; }

    public DateTimeOffset? Left { get; set; }

    public int? UnreadMessages { get; set; }

    public bool IsMuted { get; set; }

    public DateTime? MutedUntil { get; set; }

    public DateTimeOffset? Added { get; set; }

    public UserId? AddedById { get; set; }

    public DateTimeOffset? Removed { get; set; }

    public UserId? RemovedById { get; set; }

    public DateTimeOffset? Blocked { get; set; }

    public UserId? BlockedById { get; set; }
}