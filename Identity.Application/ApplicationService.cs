using AutoMapper;
using DDD.Identity.SeedWork;

namespace DDD.Identity;

public abstract class ApplicationService
{
    protected IUnitOfWork UnitOfWork { get; }
    protected IMapper Mapper { get; }

    protected ApplicationService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        UnitOfWork = unitOfWork;
        Mapper = mapper;
    }
}