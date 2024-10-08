﻿using YourBrand.Domain;
using YourBrand.Tenancy;

namespace YourBrand.Catalog.Domain.Entities;

public class ProductAttribute : Entity<string>, IHasTenant, IHasOrganization
{
    public ProductAttribute() : base(Guid.NewGuid().ToString())
    {
    }

    public TenantId TenantId { get; set; }

    public OrganizationId OrganizationId { get; set; }

    public int ProductId { get; set; }

    public Product Product { get; set; } = null!;

    public string AttributeId { get; set; } = null!;

    public Entities.Attribute Attribute { get; set; } = null!;

    public bool ForVariant { get; set; }

    public bool IsMainAttribute { get; set; }

    public AttributeValue? Value { get; set; } = null!;
}