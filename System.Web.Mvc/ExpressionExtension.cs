namespace System.Web.Mvc
{
    using Linq.Expressions;


    public static class ExpressionExtension
    {
        /// <summary>
        /// Convert lambda expression to property name
        /// </summary>
        /// <typeparam name="TModel">Current ViewModel</typeparam>
        /// <typeparam name="TItem">ViewModel Item</typeparam>
        /// <param name="expression">Lambda expression of property value</param>
        /// <returns>Property value string</returns>
        public static string ToProperty<TModel, TItem>(this Expression<Func<TModel, TItem>> expression)
        {
            // v.1.4
            return ExpressionHelper.GetExpressionText(expression);

            // v.1.3c
            //var lambda = expression as LambdaExpression;
            //var expression = lambda.Body.ToString();
            //return expression.Substring(expression.IndexOf('.') + 1);

            // v.1.2
            //// return property name only
            //var lambda = expression as LambdaExpression;
            //MemberExpression memberExpression;
            //if (lambda.Body is UnaryExpression) 
            //{
            //  var unaryExpression = lambda.Body as UnaryExpression;
            //  memberExpression = unaryExpression.Operand as MemberExpression;
            //}
            //else
            //  memberExpression = lambda.Body as MemberExpression;
            //var propertyInfo = memberExpression.Member as PropertyInfo;
            //return propertyInfo.Name;
        }

    }
}