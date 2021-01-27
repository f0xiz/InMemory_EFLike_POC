using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DBCore;
using DBCore.Model;

namespace DBCorev2
{
    public abstract class InMemoryDB
    {
        private Dictionary<Type, IInMemoryTable> _tables = new();
        private InMemoryTableFactory _tableFactory = new();
        private Dictionary<Type, EntityType> _model;
        private readonly ModelBuilder _modelBuilder;

        protected InMemoryDB()
        {
            _modelBuilder = new ModelBuilder();
            OnModelCreating(_modelBuilder);
            _model = _modelBuilder.Build();
            
            InitializeSets();
            
            foreach (var entityType in _model.Values)
            {
                var key = entityType.Type;
                if (!_tables.TryGetValue(key, out var table))
                {
                    _tables.Add(key, _tableFactory.Create(entityType));
                }
            }
        }

        public abstract void OnModelCreating(ModelBuilder modelBuilder);

        public void InitializeSets()
        {
            foreach (var setInfo in FindSetsNonCached(GetType()))
            {
                var entityType = _model[setInfo.GenericType];
                var table = _tableFactory.Create(entityType);
                setInfo.SetterMethod.Invoke(this, new object [] {
                    table
                });
                
                _tables.Add(entityType.Type, table);
            }
        }
        
        private static DbSetProperty[] FindSetsNonCached(Type contextType)
        {
            return contextType.GetRuntimeProperties()
                .Where(
                    p => !p.GetIndexParameters().Any()
                         && p.DeclaringType != typeof(InMemoryDB)
                         && p.PropertyType.GetTypeInfo().IsGenericType
                         && p.PropertyType.GetGenericTypeDefinition() == typeof(InMemoryTable<>))
                .OrderBy(p => p.Name)
                .Select(
                    p => new DbSetProperty()
                    {
                        Name = p.Name,
                        GenericType = p.PropertyType.GenericTypeArguments.Single(),
                        SetterMethod = p.SetMethod
                    })
                .ToArray();
        }

        public void ExecuteTransaction(IList<UpdateEntry> entries)
        {
            foreach (var entry in entries)
            {
                var table = _tables[entry.entityType.Type];

                switch (entry.entryState)
                {
                    case EntityState.Added:
                        table.Add(entry.entry);
                        break;
                }
            }
        }

        public void Add<TEntity>(TEntity entity)
        {
            var entityType = _model[typeof(TEntity)];
            ExecuteTransaction(new List<UpdateEntry>()
            {
                new UpdateEntry()
                {
                    entityType = entityType,
                    entryState = EntityState.Added,
                    entry = entity
                }
            });
        }

        public TEntity Find<TEntity>(object key)
        {
            var table = _tables[typeof(TEntity)];
            return (TEntity)table.Find(key);
        }

        public ICollection<TEntity> Find<TEntity>(Expression<Func<TEntity, object>> indexExpression, object indexKey)
        {
            var property = (PropertyInfo)indexExpression.GetMemberAccessList().First();
            var table = _tables[typeof(TEntity)];
            return table.Find(property, indexKey).Select(o => (TEntity)o).ToList();
        }
    }
}