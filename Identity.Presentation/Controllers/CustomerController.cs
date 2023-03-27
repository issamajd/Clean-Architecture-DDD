using DDD.Authorization.AspNetCore;
using DDD.Identity.Customers;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = Roles.Admin)]
    public async Task<CustomerDto> GetById(Guid id)
    {
        return await _customerAppService.GetAsync(id);
    }

    [Route("register")]
    [HttpPut]
    public async Task<CustomerDto> Register(RegisterCustomerAccountDto registerCustomerAccountDto)
    {
        return await _customerAppService.CreateAsync(registerCustomerAccountDto);
    }

    [Route("change-age")]
    [HttpPost]
    [Authorize(Roles = Roles.Customer)]
    [Permission("Identity.Customer.Read")]
    public async Task<CustomerDto> ChangeAge(ChangeCustomerAgeDto changeCustomerAgeDto)
    {
        return await _customerAppService.ChangeCustomerAgeAsync(changeCustomerAgeDto);
    }
}