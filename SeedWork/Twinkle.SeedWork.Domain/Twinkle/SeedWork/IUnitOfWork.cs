namespace Twinkle.SeedWork;

public interface IUnitOfWork
{
    Task<IUnitOfWorkSaveHandle> BeginAsync();
   
}