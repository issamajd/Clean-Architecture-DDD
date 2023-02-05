namespace DDD.Customers;

public class Customer 
{
    public Guid Id { get; }
    public Guid UserId { get; }
    public int? Age { get; set; }

    private Customer()
    {
    }

    public Customer(Guid id, Guid userId, int? age = null)
    {
        Id = id;
        UserId = userId;
        Age = age;
    }
}