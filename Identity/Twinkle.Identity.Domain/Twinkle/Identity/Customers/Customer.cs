using System.ComponentModel.DataAnnotations;
using Twinkle.SeedWork;

namespace Twinkle.Identity.Customers;

public sealed class Customer : AggregateRoot<Guid>
{
    public int? Age { get; set; }

    [Required] public Guid UserId { get; }

    public Customer(Guid id, Guid userId, int? age = null)
    {
        Id = id;
        Age = age;
        UserId = userId;
    }
}