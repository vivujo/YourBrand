using System.Diagnostics.CodeAnalysis;

namespace YourBrand.Domain;

public struct OrganizationId
{
    public OrganizationId(string value) => Value = value;

    public string Value { get; private set; }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return Value?.GetHashCode() ?? string.Empty.GetHashCode();
    }

    public override string ToString()
    {
        return Value?.ToString() ?? string.Empty;
    }

    public static bool operator ==(OrganizationId lhs, OrganizationId rhs) => lhs.Value == rhs.Value;

    public static bool operator !=(OrganizationId lhs, OrganizationId rhs) => lhs.Value != rhs.Value;

    public static implicit operator OrganizationId(string id) => id is null ? default : new OrganizationId(id);

    public static implicit operator OrganizationId?(string? id) => id is null ? (OrganizationId?)null : new OrganizationId(id);

    public static implicit operator string(OrganizationId id) => id.Value;
}