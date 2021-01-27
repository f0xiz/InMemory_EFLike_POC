using System;
using System.Reflection;

namespace DBCorev2
{
    public class DbSetProperty
    {
        public string Name { get; set; }
        
        public Type GenericType { get; set; }

        public MethodInfo SetterMethod { get; set; }
    }
}