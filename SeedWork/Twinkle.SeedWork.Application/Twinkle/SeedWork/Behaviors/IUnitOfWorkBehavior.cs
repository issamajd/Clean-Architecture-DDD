namespace Twinkle.SeedWork.Behaviors;

public interface IUnitOfWorkBehavior
{
    public Task ExecuteAsUnitOfWorkAsync(Func<Task> func);
    
    public Task<TResult> ExecuteAsUnitOfWorkAsync<TResult>(Func<Task<TResult>> func);
}