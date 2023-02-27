using AutoMapper;
using DDD.Identity.Customers;
using DDD.Identity.Providers;

namespace DDD.Identity;

public class IdentityServiceAutoMapperProfile : Profile
{
    public IdentityServiceAutoMapperProfile() {
        CreateMap<Customer, CustomerDto>();
        CreateMap<Provider, ProviderDto>();
    }
}