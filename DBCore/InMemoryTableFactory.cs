using System;
using System.Collections.Concurrent;
using System.Reflection;
using DBCore;

namespace DBCorev2
{
    public class InMemoryTableFactory
    {
        private readonly ConcurrentDictionary<EntityType, Func<IInMemoryTable>> _factories = new();
        
        public virtual IInMemoryTable Create(EntityType entityType)
            => _factories.GetOrAdd(entityType, e => CreateTable(e))();

        private Func<IInMemoryTable> CreateTable(EntityType entityType)
            => (Func<IInMemoryTable>)typeof(InMemoryTableFactory).GetTypeInfo()
                .GetDeclaredMethod(nameof(CreateFactory))
                .MakeGenericMethod(entityType.Type)
                .Invoke(null, new object[] { entityType});
        
        private static Func<IInMemoryTable> CreateFactory<TValue>(EntityType entityType)
            => () => new InMemoryTable<TValue>(entityType);
    }
}