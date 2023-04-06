using System.Linq.Expressions;

namespace Twinkle.SeedWork.Specifications;
internal static class ExpressionExtensions
{
    private static Expression ReplaceParameter<T>(Expression<Func<T, bool>> expression,
        ParameterExpression newParameterExpression)
    {
        var replaceExpressionVisitor = new ReplaceExpressionVisitor(expression.Parameters[0], newParameterExpression);

        var expressionRevisited = replaceExpressionVisitor.Visit(expression.Body);
        if (expressionRevisited == null)
            throw new NullReferenceException(nameof(expressionRevisited));
        return expressionRevisited;
    }

    public static Expression<Func<T, bool>>
        And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    {
        //change parameter names in the second expression if it varies
        var parameter = Expression.Parameter(typeof(T), first.Parameters[0].Name);
        var secondRevisited = ReplaceParameter(second, parameter);
        return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(first, secondRevisited), parameter);
    }

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        //change parameter names in the second expression if it varies
        var parameter = Expression.Parameter(typeof(T), first.Parameters[0].Name);
        var secondRevisited = ReplaceParameter(second, parameter);

        return Expression.Lambda<Func<T, bool>>(
            Expression.OrElse(first, secondRevisited), parameter);
    }

    private class ReplaceExpressionVisitor
        : ExpressionVisitor
    {
        private readonly Expression _oldValue;
        private readonly Expression _newValue;

        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override Expression? Visit(Expression? node) => node == _oldValue ? _newValue : base.Visit(node);
    }
}