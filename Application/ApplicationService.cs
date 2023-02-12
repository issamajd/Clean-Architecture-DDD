using DDD.SeedWork;

namespace DDD;

public abstract class ApplicationService
{
    protected IUnitOfWork UnitOfWork { get; }

    protected ApplicationService(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }
}