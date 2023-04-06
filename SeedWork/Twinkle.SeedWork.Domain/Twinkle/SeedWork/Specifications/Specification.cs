using System.Linq.Expressions;

namespace Twinkle.SeedWork.Specifications;

public abstract class Specification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();

    /// <summary>
    /// Check if the provided entity satisfies the expression 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>true if the specification expression is satisfied </returns>
    public bool IsSatisfiedBy(T obj) => ToExpression().Compile()(obj);

    /// <summary>
    /// Implicitly converts a specification to expression.
    /// </summary>
    /// <param name="specification"></param>
    public static implicit operator Expression<Func<T, bool>>(Specification<T> specification)
        => specification.ToExpression();
}