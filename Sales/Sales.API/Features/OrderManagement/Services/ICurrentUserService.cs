﻿namespace YourBrand.Sales.Features.Services;

public interface ICurrentUserService
{
    string? UserId { get; }

    string? OrganizationId { get; }
}