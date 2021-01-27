using System.Collections.Generic;
using DBCore;
using DBCore.Model;

namespace DBCorev2
{
    public class InMemoryIndex<TKey, TValue, TIndexKey> : IInMemoryIndex<TValue>
    {
        private readonly EntityType _entityType;
        private readonly IndexModel _indexModel;
        public Dictionary<TIndexKey, Dictionary<TKey, TValue>> _storage = new();
        
        public InMemoryIndex(EntityType entityType, IndexModel indexModel)
        {
            _entityType = entityType;
            _indexModel = indexModel;
        }
        
        public void UpdateIndex(TValue value)
        {
            var key = CreateKey(value);
            var indexKey = CreateIndexKey(value);
            _storage.TryGetValue(indexKey, out var indexValues);
            if (indexValues == null)
            {
                indexValues = new Dictionary<TKey, TValue>();
                _storage[indexKey] = indexValues;
            }
            indexValues[key] = value;
            
        }

        public ICollection<TValue> Get(object indexKey)
        {
            if (_storage.TryGetValue((TIndexKey)indexKey, out var values))
            {
                return values.Values;
            }

            return new List<TValue>();
        }
        
        private TKey CreateKey(TValue value)
        {
            return _entityType.KeyProperty.GetValue<TValue, TKey>(value);
        }
        
        private TIndexKey CreateIndexKey(TValue value)
        {
            return _indexModel.IndexProperty.GetValue<TValue, TIndexKey>(value);
        }
    }
}