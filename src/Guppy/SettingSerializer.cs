using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public abstract class SettingSerializer
    {
        public readonly Type Type;

        internal SettingSerializer(Type type)
        {
            this.Type = type;
        }
    }

    public sealed class SettingSerializer<T> : SettingSerializer
    {
        public readonly Func<T, string> Serialize;
        public readonly Func<string, T> Deserialize;

        public SettingSerializer(Func<T, string> serialize, Func<string, T> deserialize) : base(typeof(T))
        {
            this.Serialize = serialize;
            this.Deserialize = deserialize;
        }
    }
}
