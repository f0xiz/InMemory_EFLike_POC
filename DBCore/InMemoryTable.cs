using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DBCore;
using DBCore.Model;

namespace DBCorev2
{
    public class InMemoryTable<TValue> : IInMemoryTable
    {
        private readonly EntityType _entityType;
        private readonly IInMemoryIndex<TValue> _keyIndex;
        private readonly Dictionary<Type, IInMemoryIndex<TValue>> _indexes = new();
        private readonly InMemoryIndexFactory _indexFactory = new ();
        
        public InMemoryTable(EntityType entityType)
        {
            _entityType = entityType;

            _keyIndex = _indexFactory.Create<TValue>(entityType, new IndexModel()
            {
                IndexProperty = entityType.KeyProperty,
                IsUnique = true
            });
            
            foreach (var indexModel in entityType.Indexes.Values)
            {
                _indexes.Add(indexModel.IndexProperty.PropertyType, _indexFactory.Create<TValue>(entityType, indexModel));
            }
        }

        public void Add(object initialValue)
        {
            var value = (TValue) initialValue;
            _keyIndex.UpdateIndex(value);
            foreach (var index in _indexes.Values)
            {
                index.UpdateIndex(value);
            }
        }
        
        public void Add(TValue initialValue)
        {
            var value = initialValue;
            _keyIndex.UpdateIndex(value);
            foreach (var index in _indexes.Values)
            {
                index.UpdateIndex(value);
            }
        }
        
        public object Find(object key)
        {
            return _keyIndex.Get(key).Single();
        }
        
        public IEnumerable<object> Find(PropertyInfo propertyInfo, object value)
        {
            _indexes.TryGetValue(propertyInfo.PropertyType, out var index);
            if (index != null)
            {
                return index.Get(value).Select(o => (object)o);
            }

            return new List<object>();
        }
        
        public TValue FindByKey<TKey>(TKey key)
        {
            return _keyIndex.Get(key).Single();
        }
        
        public IEnumerable<TValue> FindByIndex<TProperty>(Expression<Func<TValue, TProperty>> indexExpression, TProperty value)
        {
             _indexes.TryGetValue(typeof(TProperty), out var index);
              if (index != null)
              {
                  return index.Get(value);
              }

              return new List<TValue>();
        }
    }
}