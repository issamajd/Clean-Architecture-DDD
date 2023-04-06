using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Twinkle.Identity.AppUsers;
using Twinkle.SeedWork;

namespace Twinkle.Identity.Customers;

public class CustomerAppService : ApplicationService, ICustomerAppService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IIdentityService<Guid> _identityService;
    private readonly ILogger<CustomerAppService> _logger;

    public CustomerAppService(
        ICustomerRepository customerRepository,
        UserManager<AppUser> userManager,
        IIdentityService<Guid> identityService,
        ILogger<CustomerAppService> logger)
    {
        _customerRepository = customerRepository;
        _userManager = userManager;
        _identityService = identityService;
        _logger = logger;
    }

    public async Task<CustomerDto> GetAsync(Guid id)
    {
        var customer = await _customerRepository.GetAsync(customer => customer.Id == id);
        return customer.Adapt<CustomerDto>();
    }


    public async Task<CustomerDto> CreateAsync(RegisterCustomerAccountDto registerCustomerAccountDto)
    {
        var user = new AppUser(id: Guid.NewGuid(),
            username: registerCustomerAccountDto.Username,
            email: registerCustomerAccountDto.Email);

        // TODO move this logic to CustomerManager
        var result = await _userManager.CreateAsync(user, registerCustomerAccountDto.Password);
        if (!result.Succeeded)
            throw new InvalidOperationException(
                $"Unable to create a user: {result.Errors.FirstOrDefault()?.Description}");

        result = await _userManager.AddToRoleAsync(user, Roles.Customer);
        if (!result.Succeeded)
            throw new InvalidOperationException(
                $"Unable to add role user: {result.Errors.FirstOrDefault()?.Description}");
        var customer = new Customer(id: Guid.NewGuid(), userId: user.Id, age: registerCustomerAccountDto.Age);
        customer = await _customerRepository.AddAsync(customer);
        return customer.Adapt<CustomerDto>();
    }

    public async Task<CustomerDto> ChangeCustomerAgeAsync(ChangeCustomerAgeDto changeCustomerAgeDto)
    {
        var userId = _identityService.GetUserId();

        var customer = await _customerRepository.GetAsync(customer => customer.UserId == userId);
        customer.Age = changeCustomerAgeDto.Age;
        customer = _customerRepository.Update(customer);

        return customer.Adapt<CustomerDto>();
    }
}