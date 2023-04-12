using Twinkle.SeedWork;

namespace Twinkle.Identity.Customers;

public class EfCoreCustomerRepository : EfCoreRepository<Customer, IIdentityDbContext>, ICustomerRepository
{
    public EfCoreCustomerRepository(IIdentityDbContext dbContext) : base(dbContext)
    {
    }
}