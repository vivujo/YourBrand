﻿using System.ComponentModel.DataAnnotations.Schema;

using MediatR;

namespace YourBrand.Catalog.Domain;

[NotMapped]
public abstract record DomainEvent : INotification
{
    public Guid Id { get; } = Guid.NewGuid();

    public DateTime Timestamp { get; } = DateTime.UtcNow;
}