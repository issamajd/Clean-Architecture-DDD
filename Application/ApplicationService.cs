using AutoMapper;
using DDD.SeedWork;

namespace DDD;

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