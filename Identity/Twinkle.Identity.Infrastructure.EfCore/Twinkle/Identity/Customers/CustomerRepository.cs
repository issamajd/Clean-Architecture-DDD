using Twinkle.SeedWork;

namespace Twinkle.Identity.Customers;

public class CustomerRepository : EfCoreRepository<Customer, IIdentityDbContext>, ICustomerRepository
{
    public CustomerRepository(IIdentityDbContext dbContext) : base(dbContext)
    {
    }
}