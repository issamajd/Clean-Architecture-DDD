using DDD.AppUsers;

namespace DDD.Customers;

public sealed class Customer : AppUser
{
    public int? Age { get; set; }

    private Customer()
    {
    }

    public Customer(Guid id, string email, string username, int? age = null) : base(id, email, username)
    {
        Age = age;
    }
    
}