namespace Twinkle.SeedWork.Behaviors;

public class UnitOfWorkBehavior : IUnitOfWorkBehavior
{
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsUnitOfWorkAsync(Func<Task> func)
    {
        var saveHandle = await _unitOfWork.BeginAsync();
        await func();
        await saveHandle.CompleteAsync();
    }

    public async Task<TResult> ExecuteAsUnitOfWorkAsync<TResult>(Func<Task<TResult>> func)
    {
        var saveHandle = await _unitOfWork.BeginAsync();
        var result = await func();
        await saveHandle.CompleteAsync();
        return result;
    }
}