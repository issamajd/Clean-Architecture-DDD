using DDD.Identity.SeedWork;

namespace DDD.Identity;

public abstract class ApplicationService
{
    protected IUnitOfWork UnitOfWork { get; }

    protected ApplicationService(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }
}