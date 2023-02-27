using System.ComponentModel.DataAnnotations;
using DDD.Identity.SeedWork;

namespace DDD.Identity.Customers;

public class Customer : AggregateRoot<Guid>
{
    public int? Age { get; set; }

    [Required]
    public Guid UserId { get; }

    public Customer(Guid id, Guid userId, int? age = null)
    {
        Id = id;
        Age = age;
        UserId = userId;
    }
}