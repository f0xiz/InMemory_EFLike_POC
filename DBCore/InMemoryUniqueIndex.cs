using System.Collections.Generic;
using DBCore;
using DBCore.Model;

namespace DBCorev2
{
    public class InMemoryUniqueIndex<TValue, TIndexKey> : IInMemoryIndex<TValue>
    {
        private readonly IndexModel _indexModel;
        public Dictionary<TIndexKey, TValue> _storage = new();
        
        public InMemoryUniqueIndex(EntityType entityType, IndexModel indexModel)
        {
            _indexModel = indexModel;
        }
        
        public void UpdateIndex(TValue value)
        {
            var indexKey = CreateIndexKey(value);
            _storage[indexKey] = value;
        }

        public ICollection<TValue> Get(object indexKey)
        {
            if (_storage.TryGetValue((TIndexKey)indexKey, out var value))
            {
                return new List<TValue>(){ value };
            }

            return new List<TValue>();
        }

        private TIndexKey CreateIndexKey(TValue value)
        {
            return _indexModel.IndexProperty.GetValue<TValue, TIndexKey>(value);
        }
    }
}