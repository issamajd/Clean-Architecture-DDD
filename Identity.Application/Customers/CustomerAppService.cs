using DDD.Identity.AppUsers;
using DDD.Identity.SeedWork;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DDD.Identity.Customers;

public class CustomerAppService : ApplicationService, ICustomerAppService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomerAppService(
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager,
        IHttpContextAccessor httpContextAccessor) : base(unitOfWork)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CustomerDto> GetAsync(Guid id)
    {
        var customerRepository = UnitOfWork.Repository<ICustomerRepository, Customer>();
        var customer = await customerRepository.GetAsync(customer => customer.Id == id);
        return customer.Adapt<CustomerDto>();
    }


    public async Task<CustomerDto> CreateAsync(RegisterCustomerAccountDto registerCustomerAccountDto)
    {
        var user = new AppUser(id: Guid.NewGuid(),
            username: registerCustomerAccountDto.Username,
            email: registerCustomerAccountDto.Email);

        await UnitOfWork.BeginAsync();
        try
        {
            var result = await _userManager.CreateAsync(user, registerCustomerAccountDto.Password);
            if (!result.Succeeded)
                throw new InvalidOperationException(
                    $"Unable to create a user: {result.Errors.FirstOrDefault()?.Description}");

            result = await _userManager.AddToRoleAsync(user, Roles.Customer);
            if (!result.Succeeded)
                throw new InvalidOperationException(
                    $"Unable to add role user: {result.Errors.FirstOrDefault()?.Description}");
            var customerRepository = UnitOfWork.Repository<ICustomerRepository, Customer>();
            var customer = new Customer(id: Guid.NewGuid(), userId: user.Id, age: registerCustomerAccountDto.Age);
            customer = await customerRepository.AddAsync(customer);
            await UnitOfWork.CompleteAsync();
            return customer.Adapt<CustomerDto>();
        }
        catch (Exception)
        {
            UnitOfWork.Cancel();
            throw;
        }
    }

    public async Task<CustomerDto> ChangeCustomerAgeAsync(ChangeCustomerAgeDto changeCustomerAgeDto)
    {
        //TODO wrap accessing claims in a separate service
        var customerId = _httpContextAccessor.HttpContext.User.FindFirst("customer_id")?.Value;

        var customerRepository = UnitOfWork.Repository<ICustomerRepository, Customer>();

        var customer = await customerRepository.GetAsync(customer => customerId != null &&
                                                                     customer.Id == Guid.Parse(customerId));
        customer.Age = changeCustomerAgeDto.Age;
        customer = customerRepository.Update(customer);

        await UnitOfWork.SaveChangesAsync();
        return customer.Adapt<CustomerDto>();
    }
}