using Twinkle.SeedWork.Domain;

namespace Twinkle.SeedWork.Application.Behaviors;

public class UnitOfWorkBehavior : IUnitOfWorkBehavior
{
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task ExecuteAsUnitOfWorkAsync(Func<Task> func)
    {
        await _unitOfWork.BeginAsync();
        await func();
        await _unitOfWork.CompleteAsync();
    }

    public async Task<TResult> ExecuteAsUnitOfWorkAsync<TResult>(Func<Task<TResult>> func)
    {
        await _unitOfWork.BeginAsync();
        var result = await func();
        await _unitOfWork.CompleteAsync();
        return result;
    }
}