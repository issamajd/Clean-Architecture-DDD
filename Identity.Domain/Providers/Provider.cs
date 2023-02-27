using System.ComponentModel.DataAnnotations;
using DDD.Identity.SeedWork;

namespace DDD.Identity.Providers;

public class Provider : AggregateRoot<Guid>
{
    [Required] 
    public Guid UserId { get; }
    public string? BusinessName { get; set; }

    public Provider(Guid id, Guid userId, string? businessName = null)
    {
        Id = id;
        UserId = userId;
        BusinessName = businessName;
    }
}