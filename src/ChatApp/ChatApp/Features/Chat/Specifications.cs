﻿using YourBrand.ChatApp.Domain.Specifications;
using YourBrand.ChatApp.Domain.ValueObjects;
using YourBrand.Domain;

namespace YourBrand.ChatApp.Features.Chat;

public class InOrganization<T> : BaseSpecification<T>
    where T : IHasOrganization
{
    public InOrganization(OrganizationId organizationId)
    {
        Criteria = x => x.OrganizationId == organizationId;
    }
}

public class ChannelWithId : BaseSpecification<Channel>
{
    public ChannelWithId(ChannelId channelId)
    {
        Criteria = channel => channel.Id == channelId;
    }
}

public class ChannelWithName : BaseSpecification<Channel>
{
    public ChannelWithName(string name)
    {
        Criteria = channel => channel.Title == name;
    }
}

public class MessageWithId : BaseSpecification<Message>
{
    public MessageWithId(MessageId messageId)
    {
        Criteria = message => message.Id == messageId;
    }
}

public class MessagesInChannel : BaseSpecification<Message>
{
    public MessagesInChannel(ChannelId channelId)
    {
        Criteria = message => message.ChannelId == channelId;
    }
}