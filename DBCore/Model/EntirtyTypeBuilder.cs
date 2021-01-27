using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DBCore.Model
{
    public class EntityTypeBuilder<TEntity>
    {
        private EntityType _entityType;
        
        public EntityTypeBuilder(EntityType entityType)
        {
            _entityType = entityType;
        }

        public IndexBuilder<TEntity> HasIndex<TProperty>([NotNull] Expression<Func<TEntity, TProperty>> indexExpression)
        {
            var property = (PropertyInfo)indexExpression.GetMemberAccessList().First();

            var propertyType = typeof(TProperty);
            
            _entityType.Indexes.TryGetValue(propertyType, out var indexModel);
            if (indexModel == null)
            {
                indexModel = new IndexModel()
                {
                    IndexProperty = new Property() { PropertyInfo = property, GetExpression = indexExpression, PropertyType = propertyType}
                };
                _entityType.Indexes.Add(propertyType, indexModel);
            }

            return new IndexBuilder<TEntity>(_entityType, indexModel);
        }

        public EntityTypeBuilder<TEntity> HasKey<TProperty>(Expression<Func<TEntity, TProperty>> keyExpression)
        {
            var property = (PropertyInfo) keyExpression.GetMemberAccessList().First();
            _entityType.KeyProperty = new Property()
            {
                PropertyInfo = property,
                GetExpression = keyExpression,
                PropertyType = typeof(TProperty)
            };
            
            return this;
        }
    }
}