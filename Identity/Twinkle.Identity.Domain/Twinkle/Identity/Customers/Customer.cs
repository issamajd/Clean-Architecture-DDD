using System.ComponentModel.DataAnnotations;
using Twinkle.SeedWork;
using Twinkle.SeedWork.Auditing.Contracts;

namespace Twinkle.Identity.Customers;

public sealed class Customer : AggregateRoot<Guid>, ISoftDelete, IHasCreationTime
{
    public int? Age { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreationTime { get; private set; }
    [Required] public Guid UserId { get; }

    public Customer(Guid id, Guid userId, int? age = null)
    {
        Id = id;
        Age = age;
        UserId = userId;
    }
}