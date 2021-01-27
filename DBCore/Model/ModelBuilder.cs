using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DBCore.Model
{
    public class ModelBuilder
    {
        private Dictionary<Type, EntityType> _entityTypes = new();
        
        public EntityTypeBuilder<TEntity> Entity<TEntity>()
            where TEntity : class
        {
            _entityTypes.TryGetValue(typeof(TEntity), out var entityType);
            if (entityType == null)
            {
                entityType = new EntityType()
                {
                    Type = typeof(TEntity)
                };
                
                _entityTypes.Add(typeof(TEntity), entityType);
            }

            return new EntityTypeBuilder<TEntity>(entityType);
        }

        public Dictionary<Type,EntityType> Build()
        {
            return _entityTypes;
        }
    }
}