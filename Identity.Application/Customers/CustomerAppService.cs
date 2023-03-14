using Core;
using DDD.Identity.AppUsers;
using DDD.Identity.SeedWork;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace DDD.Identity.Customers;

public class CustomerAppService : ApplicationService, ICustomerAppService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IIdentityService<Guid> _identityService;

    public CustomerAppService(
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager,
        IIdentityService<Guid> identityService) : base(unitOfWork)
    {
        _userManager = userManager;
        _identityService = identityService;
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
        var userId = _identityService.GetUserId();
        var customerRepository = UnitOfWork.Repository<ICustomerRepository, Customer>();

        var customer = await customerRepository.GetAsync(customer => customer.UserId == userId);
        customer.Age = changeCustomerAgeDto.Age;
        customer = customerRepository.Update(customer);

        await UnitOfWork.SaveChangesAsync();
        return customer.Adapt<CustomerDto>();
    }
}