using DDD.AppUsers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DDD.Customers;

public class CustomerAppService : ICustomerAppService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomerAppService(ICustomerRepository customerRepository,
        UserManager<AppUser> userManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _customerRepository = customerRepository;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CustomerDto> GetAsync(Guid id)
    {
        var customer = await _customerRepository.GetAsync(id);
        return new CustomerDto
        {
            Age = customer.Age, Id = customer.Id
            // UserId = customer.UserId
        };
    }

    public async Task<CustomerDto> CreateAsync(RegisterCustomerAccountDto registerCustomerAccountDto)
    {
        throw new NotImplementedException();
        // var user = new AppUser()
        // {
        //     UserName = registerCustomerAccountDto.Username,
        // };
        // var result = await _userManager.CreateAsync(user, registerCustomerAccountDto.Password);
        // if (result.Succeeded)
        // {
        //     var customer = new Customer(Guid.NewGuid(), registerCustomerAccountDto.Age);
        //     customer = await _customerRepository.CreateAsync(customer);
        //
        //     if (customer != null)
        //     {
        //         // user.CustomerId = customer.Id;
        //         //TODO check if this operation is successful
        //         await _userManager.UpdateAsync(user);
        //         return new CustomerDto()
        //         {
        //             Id = customer.Id,
        //             // UserId = customer.UserId,
        //             Age = customer.Age
        //         };
        //     }
        //
        //     throw new InvalidOperationException("Unable to create customer");
        // }
        //
        // throw new InvalidOperationException($"Unable to create a user: {result.Errors.FirstOrDefault()?.Description}");
    }

    public async Task<CustomerDto> ChangeCustomerAgeAsync(ChangeCustomerAgeDto changeCustomerAgeDto)
    {
        throw new NotImplementedException();

        //TODO wrap accessing claims in a separate service
        var customerId = _httpContextAccessor.HttpContext.User.FindFirst("CustomerId").Value;
        var customer = await _customerRepository.GetAsync(Guid.Parse(customerId));
        customer = await _customerRepository.ChangeAgeAsync(customer, changeCustomerAgeDto.Age);
        return new CustomerDto
        {
            Id = customer.Id,
            // UserId = customer.UserId,
            Age = customer.Age
        };
    }
}