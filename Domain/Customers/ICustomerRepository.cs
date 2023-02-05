namespace DDD.Customers;

public interface ICustomerRepository
{
    public Task<Customer?> GetAsync(Guid id);
    
    public Task<Customer?> CreateAsync(Customer customer);
    public Task<Customer?> ChangeAgeAsync(Customer customer, int age);
}