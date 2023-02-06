using DDD.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDD.Controllers;

[Route("customer")]
public class CustomerController : Controller
{
    private readonly ICustomerAppService _customerAppService;

    public CustomerController(ICustomerAppService customerAppService)
    {
        _customerAppService = customerAppService;
    }

    [Route("/customer")]
    [HttpGet]
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
    [Authorize]
    public async Task<CustomerDto> ChangeAge(ChangeCustomerAgeDto changeCustomerAgeDto)
    {
        return await _customerAppService.ChangeCustomerAgeAsync(changeCustomerAgeDto);
    }
}