namespace DDD.Identity.Customers;

public class CustomerRepository : EfCoreRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}