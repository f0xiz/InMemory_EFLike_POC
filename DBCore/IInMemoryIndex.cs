using System.Collections.Generic;

namespace DBCorev2
{
    public interface IInMemoryIndex<TValue>
    {
        public void UpdateIndex(TValue value);

        ICollection<TValue> Get(object indexKey);
    }
}