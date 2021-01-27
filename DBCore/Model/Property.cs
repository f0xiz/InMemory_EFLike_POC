using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DBCore.Model
{
    public class Property
    {
        public Type PropertyType { get; set; }
        
        public PropertyInfo PropertyInfo { get; set; }
        public Expression GetExpression { get; set; }
        private object CompiledGet { get; set; }

        public TValue GetValue<TEntity, TValue>(TEntity entity)
        {
            if (CompiledGet == null)
            {
                var compiled = ((Expression<Func<TEntity, TValue>>) GetExpression).Compile();
                CompiledGet = compiled;
                return compiled(entity);
            }

            return ((Func<TEntity, TValue>) CompiledGet)(entity);
        }
    }
}