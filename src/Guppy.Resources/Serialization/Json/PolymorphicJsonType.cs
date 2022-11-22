using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Serialization.Json
{
    public class PolymorphicJsonType
    {
        public string Key { get; }
        public Type Type { get; }

        public PolymorphicJsonType(string key, Type type)
        {
            this.Key = key;
            this.Type = type;
        }
    }

    public sealed class PolymorphicJsonType<T> : PolymorphicJsonType
    {
        public PolymorphicJsonType(string key) : base(key, typeof(T))
        {
        }
    }
}
