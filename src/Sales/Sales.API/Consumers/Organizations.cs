﻿using YourBrand.IdentityManagement.Contracts;
using YourBrand.Sales.Features.Services;
using YourBrand.Sales.Features.OrderManagement.Users;

using MassTransit;

using MediatR;
using YourBrand.Sales.Features.OrderManagement.Organizations;
using YourBrand.Sales.Features.Common;

namespace YourBrand.Sales.Consumers;

public class SalesOrganizationCreatedConsumer : IConsumer<OrganizationCreated>
{
    private readonly IMediator _mediator;
    private readonly IRequestClient<GetOrganization> _requestClient;
    private readonly ILogger<SalesOrganizationCreatedConsumer> _logger;
    private readonly ICurrentUserService _currentUserService;

    public SalesOrganizationCreatedConsumer(IMediator mediator, ICurrentUserService currentUserService, IRequestClient<GetOrganization> requestClient, ILogger<SalesOrganizationCreatedConsumer> logger)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
        _requestClient = requestClient;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrganizationCreated> context)
    {
        try 
        {
            var message = context.Message;

            //_currentUserService.SetCurrentUser(message.CreatedById);

            var result = await _mediator.Send(new Sales.Features.OrderManagement.Organizations.CreateOrganization(message.OrganizationId, message.Name, message.TenantId));
        }
        catch(Exception e) 
        {
            _logger.LogError(e, "FOO"); 
        }
    }
}

public class SalesOrganizationDeletedConsumer : IConsumer<OrganizationDeleted>
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public SalesOrganizationDeletedConsumer(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    public async Task Consume(ConsumeContext<OrganizationDeleted> context)
    {
        var message = context.Message;

        //_currentUserService.SetCurrentUser(message.DeletedById);

        await _mediator.Send(new DeleteUser(message.OrganizationId));
    }
}


public class SalesOrganizationUpdatedConsumer : IConsumer<OrganizationUpdated>
{
    private readonly IMediator _mediator;
    private readonly IRequestClient<GetOrganization> _requestClient;
    private readonly ICurrentUserService _currentUserService;

    public SalesOrganizationUpdatedConsumer(IMediator mediator, IRequestClient<GetOrganization> requestClient, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _requestClient = requestClient;
        _currentUserService = currentUserService;
    }

    public async Task Consume(ConsumeContext<OrganizationUpdated> context)
    {
        var message = context.Message;

        //_currentUserService.SetCurrentUser(message.UpdatedById);

        var messageR = await _requestClient.GetResponse<GetOrganizationResponse>(new GetUser(message.OrganizationId, message.UpdatedById));
        var message2 = messageR.Message;

        var result = await _mediator.Send(new UpdateOrganization(message2.Id, message2.Name));
    }
}

public class SalesOrganizationUserAddedConsumer : IConsumer<OrganizationUserAdded>
{
    private readonly IMediator _mediator;
    private readonly ITenantService _tenantService;

    public SalesOrganizationUserAddedConsumer(IMediator mediator, ITenantService tenantService)
    {
        _mediator = mediator;
        _tenantService = tenantService;
    }

    public async Task Consume(ConsumeContext<OrganizationUserAdded> context)
    {
        var message = context.Message;

        _tenantService.SetTenantId(message.TenantId);

        var result = await _mediator.Send(new AddUserToOrganization(message.OrganizationId, message.UserId));
    }
}