using System;
using System.Collections.Generic;
using DBCore.Model;

namespace DBCore
{
    public class EntityType
    {
        public Type Type { get; set; }
        
        public Property KeyProperty = new();

        public Dictionary<Type, IndexModel> Indexes = new();
    }
}