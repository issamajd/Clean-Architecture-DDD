using System.Linq.Expressions;

namespace Twinkle.SeedWork.Specifications;

public class OrSpecification<T> : Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    public OrSpecification(Specification<T> left, Specification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override Expression<Func<T, bool>> ToExpression()
        => _left.ToExpression().Or(_right.ToExpression());
}