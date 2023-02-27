using AutoMapper;
using DDD.Customers;
using DDD.Providers;

namespace DDD;

public class IdentityServiceAutoMapperProfile : Profile
{
    public IdentityServiceAutoMapperProfile() {
        CreateMap<Customer, CustomerDto>();
        CreateMap<Provider, ProviderDto>();
    }
}