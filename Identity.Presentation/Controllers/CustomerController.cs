using DDD.Authorization.AspNetCore;
using DDD.Identity.Customers;
using DDD.Identity.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace DDD.Identity.Presentation.Controllers;

[ApiController]
[Route("customer")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerAppService _customerAppService;

    public CustomerController(ICustomerAppService customerAppService)
    {
        _customerAppService = customerAppService;
    }

    [HttpGet]
    [Permission(IdentityPermissions.Customers.Default)]
    public Task<CustomerDto> GetById(Guid id) => _customerAppService.GetAsync(id);


    [Route("register")]
    [HttpPut]
    public Task<CustomerDto> Register(RegisterCustomerAccountDto registerCustomerAccountDto) =>
        _customerAppService.CreateAsync(registerCustomerAccountDto);


    [Route("change-age")]
    [HttpPost]
    [Permission(IdentityPermissions.Customers.Edit)]
    public Task<CustomerDto> ChangeAge(ChangeCustomerAgeDto changeCustomerAgeDto) =>
        _customerAppService.ChangeCustomerAgeAsync(changeCustomerAgeDto);
}