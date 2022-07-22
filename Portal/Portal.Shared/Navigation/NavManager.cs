﻿namespace YourBrand.Portal.Navigation;

public class NavManager
{
    private List<NavGroup> _groups = new List<NavGroup>();

    public IReadOnlyList<NavGroup> Groups => _groups;

    public NavGroup? GetGroup(string id) => Groups.FirstOrDefault(g => g.Id == id);

    public NavGroup CreateGroup(string id, string name)
    {
        var navGroup = new NavGroup()
        {
            Id = id,
            Name = name
        };
        _groups.Add(navGroup);

        Updated?.Invoke(this, EventArgs.Empty);

        return navGroup;
    }

    public NavGroup CreateGroup(string id, Func<string> name)
    {
        var navGroup = new NavGroup()
        {
            Id = id,
            NameFunc = name
        };
        _groups.Add(navGroup);

        Updated?.Invoke(this, EventArgs.Empty);

        return navGroup;
    }

    public NavGroup CreateGroup(string id, Action<NavGroupOptions> setup)
    {
        NavGroupOptions options = new NavGroupOptions();
        setup(options);

        var navGroup = new NavGroup()
        {
            Id = id,
            Name = options.Name,
            NameFunc = options.NameFunc,
            RequiresAuthorization = options.RequiresAuthorization,
            Roles = options.Roles
        };
        _groups.Add(navGroup);

        Updated?.Invoke(this, EventArgs.Empty);

        return navGroup;
    }

    public event EventHandler? Updated;
}

public class NavGroupOptions
{
    public string Name { get; set; }

    public Func<string> NameFunc { get; set; }

    //public string Icon { get; set; }

    //public string Href { get; set; }

    public bool RequiresAuthorization { get; set; }

    public IEnumerable<string>? Roles { get; set; }
}