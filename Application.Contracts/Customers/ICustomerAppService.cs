namespace DDD.Customers;

public interface ICustomerAppService
{
    Task<CustomerDto> GetAsync(Guid id);
    Task<CustomerDto> CreateAsync(RegisterCustomerAccountDto registerCustomerAccountDto);
    
    Task<CustomerDto> ChangeCustomerAgeAsync(ChangeCustomerAgeDto changeCustomerAgeDto);
}