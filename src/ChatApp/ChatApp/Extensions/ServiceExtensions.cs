﻿using FluentValidation;

using MediatR;

using YourBrand.ChatApp.Behaviors;
using YourBrand.ChatApp.Features.Chat;

namespace YourBrand.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining(typeof(ServiceExtensions)));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(typeof(ServiceExtensions).Assembly);

        return services;
    }

    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddTodoControllers();

        services.AddScoped<IChatNotificationService, ChatNotificationService>();

        return services;
    }
}