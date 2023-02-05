using DDD.EfCore;

namespace DDD.Customers;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _dbContext;

    public CustomerRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Customer?> GetAsync(Guid id)
    {
        var customer = await _dbContext.Customers.FindAsync(id);
        return customer;
    }

    public async Task<Customer?> CreateAsync(Customer customer)
    {
        var dbSet = _dbContext.Set<Customer>();
        var customerEntity = await dbSet.AddAsync(customer);
        await _dbContext.SaveChangesAsync();
        return customerEntity.Entity;
    }

    public async Task<Customer> ChangeAgeAsync(Customer customer, int age)
    {
        var dbSet = _dbContext.Set<Customer>();
        dbSet.Attach(customer);
        customer.Age = age;
        await _dbContext.SaveChangesAsync();
        return customer;
    }
}