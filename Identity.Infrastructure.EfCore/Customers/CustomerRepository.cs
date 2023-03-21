using DDD.Core.Infrastructure.EfCore;

namespace DDD.Identity.Customers;

public class CustomerRepository : EfCoreRepository<Customer, IIdentityDbContext>, ICustomerRepository
{
    public CustomerRepository(IIdentityDbContext dbContext) : base(dbContext)
    {
    }
}