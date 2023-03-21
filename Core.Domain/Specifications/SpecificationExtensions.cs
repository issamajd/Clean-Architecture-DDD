using DDD.Core.Utils;

namespace DDD.Core.Domain.Specifications;

public static class SpecificationExtensions
{
    public static Specification<T> And<T>(this Specification<T> specification,
        Specification<T> other)
    {
        Check.NotNull(specification, nameof(specification));
        Check.NotNull(other, nameof(other));

        return new AndSpecification<T>(specification, other);
    }

    public static Specification<T> Or<T>(this Specification<T> specification,
        Specification<T> other)
    {
        Check.NotNull(specification, nameof(specification));
        Check.NotNull(other, nameof(other));

        return new OrSpecification<T>(specification, other);
    }
}