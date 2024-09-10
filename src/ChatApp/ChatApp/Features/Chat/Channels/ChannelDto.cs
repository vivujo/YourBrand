using YourBrand.ChatApp.Features.Users;
using YourBrand.ChatApp.Domain.ValueObjects;

namespace YourBrand.ChatApp.Features.Chat.Channels;

public sealed record ChannelDto(string Id, string Name);

public sealed record ParticipantDto(string Id, string ChannelId, string Name, string? UserId);