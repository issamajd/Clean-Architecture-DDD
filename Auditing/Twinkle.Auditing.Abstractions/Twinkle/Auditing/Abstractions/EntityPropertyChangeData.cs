namespace Twinkle.Auditing.Abstractions;

public class EntityPropertyChangeData
{
    public string? OldValue { get; set; }
    public string NewValue { get; set; }
    public string PropertyName { get; set; }
    public string PropertyType { get; set; }
}