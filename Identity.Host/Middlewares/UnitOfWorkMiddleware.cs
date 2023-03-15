using DDD.Identity.Behaviors;

namespace DDD.Identity.Middlewares;

public class UnitOfWorkMiddleware
{
    private readonly RequestDelegate _next;

    public UnitOfWorkMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUnitOfWorkBehavior unitOfWorkBehavior)
    {
        if (context.Request.Method == HttpMethod.Get.Method)
        {
            await _next(context);
            return;
        }

        await unitOfWorkBehavior.ExecuteAsUnitOfWorkAsync(() => _next(context));
    }
}