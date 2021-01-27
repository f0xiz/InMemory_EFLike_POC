using System;
using System.Reflection;
using DBCore;
using DBCore.Model;

namespace DBCorev2
{
    public class InMemoryIndexFactory
    {
        public virtual IInMemoryIndex<TValue> Create<TValue>(EntityType entityType, IndexModel indexModel) 
            => CreateIndex<TValue>(entityType, indexModel)();

        private Func<IInMemoryIndex<TValue>> CreateIndex<TValue>(EntityType entityType, IndexModel indexModel)
            => (Func<IInMemoryIndex<TValue>>)typeof(InMemoryIndexFactory).GetTypeInfo()
                .GetDeclaredMethod(nameof(CreateFactory))
                .MakeGenericMethod(entityType.KeyProperty.PropertyInfo.PropertyType,
                    entityType.Type,
                    indexModel.IndexProperty.PropertyType)
                .Invoke(null, new object[] { entityType, indexModel });
        
        private static Func<IInMemoryIndex<TValue>> CreateFactory<TKey, TValue, TIndexKey>(EntityType entityType, IndexModel indexModel)
            => () =>
            {
                if (indexModel.IsUnique)
                {
                    return new InMemoryUniqueIndex<TValue, TIndexKey>(entityType, indexModel);
                }
                return new InMemoryIndex<TKey, TValue, TIndexKey>(entityType, indexModel);
            };
    }
}