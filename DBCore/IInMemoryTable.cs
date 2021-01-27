using System.Collections.Generic;
using System.Reflection;

namespace DBCorev2
{
    public interface IInMemoryTable
    {
        void Add(object entry);

        object Find(object key);

        IEnumerable<object> Find(PropertyInfo propertyInfo, object value);
    }
}