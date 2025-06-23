namespace Sentyll.Core.Services.Extensions;

public static class PredicateBuilderExtensions
{
    
    public static Expression<Func<T,bool>> AndIf<T>(
        this ExpressionStarter<T> expr, 
        bool applyCondition, 
        Expression<Func<T,bool>> andCondition) 
        => applyCondition
            ? expr.And(andCondition)
            : expr;
    
    public static Expression<Func<T,bool>> AndIf<T>(
        this ExpressionStarter<T> expr, 
        bool applyCondition, 
        Func<Expression<Func<T,bool>>> andCondition)
    {
        if (applyCondition)
        {
            var andExpression = andCondition();
            return expr.And(andExpression);
        }
        
        return expr;
    }
}