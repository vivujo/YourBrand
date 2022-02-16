﻿using System;

using Skynet.Application;
using Skynet.Application.Common.Interfaces;
using Skynet.Infrastructure;

using Skynet.WebApi.Hubs;
using Skynet.WebApi.Services;

namespace Skynet.WebApi;

public static class ServiceExtensions
{
    public  static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IUrlHelper, UrlHelper>();

        services.AddClients();

        services.AddScoped<IFileUploaderService, FileUploaderService>();

        return services;
    }

    private static IServiceCollection AddClients(this IServiceCollection services)
    {
        services.AddScoped<IItemsClient, ItemsClient>();
        services.AddScoped<IWorkerClient, WorkerClient>();
        services.AddScoped<INotificationClient, NotificationClient>();
        services.AddScoped<ISomethingClient, SomethingClient>();

        return services;
    }
}