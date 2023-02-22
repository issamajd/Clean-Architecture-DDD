using DDD.AppUsers;
using DDD.SeedWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DDD.Customers;

public class CustomerAppService : ApplicationService, ICustomerAppService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly AppUserManager _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomerAppService(IUnitOfWork unitOfWork, ICustomerRepository customerRepository,
        AppUserManager userManager,
        IHttpContextAccessor httpContextAccessor) : base(unitOfWork)
    {
        _customerRepository = customerRepository;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CustomerDto> GetAsync(Guid id)
    {
        var customer = await _customerRepository.GetAsync(customer => customer.Id == id);
        return new CustomerDto
        {
            Id = customer.Id,
            Age = customer.Age,
            UserId = customer.UserId
        };
    }

    public async Task<CustomerDto> CreateAsync(RegisterCustomerAccountDto registerCustomerAccountDto)
    {
        var user = new AppUser(id: Guid.NewGuid(), username: registerCustomerAccountDto.Username);

        var result = await _userManager.CreateUserWithRolesAsync(user, registerCustomerAccountDto.Password, new[] {Roles.Customer});
        if (!result.Succeeded)
            throw new InvalidOperationException(
                $"Unable to create a user: {result.Errors.FirstOrDefault()?.Description}");
        
        var customer = new Customer(id: Guid.NewGuid(), userId: user.Id, age: registerCustomerAccountDto.Age);
        customer = await _customerRepository.InsertAsync(customer);
        await UnitOfWork.SaveChangesAsync();
        return new CustomerDto
        {
            Id = customer.Id,
            UserId = customer.UserId,
            Age = customer.Age
        };
    }

    public async Task<CustomerDto> ChangeCustomerAgeAsync(ChangeCustomerAgeDto changeCustomerAgeDto)
    {
        //TODO wrap accessing claims in a separate service
        var customerId = _httpContextAccessor.HttpContext.User.FindFirst("customer_id")?.Value;
        var customer = await _customerRepository.GetAsync(customer => customerId != null &&
                                                                      customer.Id == Guid.Parse(customerId));
        customer.Age = changeCustomerAgeDto.Age;
        customer = _customerRepository.Update(customer);
        await UnitOfWork.SaveChangesAsync();

        return new CustomerDto
        {
            Id = customer.Id,
            UserId = customer.UserId,
            Age = customer.Age
        };
    }
}