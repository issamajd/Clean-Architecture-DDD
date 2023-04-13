using Twinkle.SeedWork;

namespace Twinkle.Authorization.Abstractions;

public class Permission
{
    public string Name { get; }

    public string? ParentName { get; }

    public string DisplayName { get; }

    private List<Permission> Children { get; }

    internal Permission(string name, string? parentName = null, string? displayName = null)
    {
        Name = Check.NotNullOrEmpty(name, nameof(name));
        ParentName = parentName;
        DisplayName = displayName ?? name;
        Children = new List<Permission>();
    }

    public Permission AddChild(string name, string? displayName = null)
    {
        var permission = new Permission(name, Name, displayName: displayName);
        Children.Add(permission);
        return permission;
    }

    /// <summary>
    /// Get current permission descendants 
    /// </summary>
    /// <returns>Enumerable of <see cref="Permission"/></returns>
    public IEnumerable<Permission> GetDescendants()
    {
        var descendants = new List<Permission>(Children);
        var visited = new HashSet<string>();
        int currentDescendantsCount;
        do
        {
            currentDescendantsCount = descendants.Count;
            var unvisitedDescendants =
                descendants.Where(predicateDescendant => !visited.Contains(predicateDescendant.Name)).ToList();
            foreach (var descendant in unvisitedDescendants)
            {
                visited.Add(descendant.Name);
                descendants.AddRange(descendant.Children);
            }
        } while (descendants.Count > currentDescendantsCount);

        return descendants;
    }

    /// <summary>
    /// return the direct children 
    /// </summary>
    /// <returns>Enumerable of <see cref="Permission"/></returns>
    public IEnumerable<Permission> GetChildren() => Children;
}