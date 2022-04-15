using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Settings.Loaders.Descriptors
{
    public abstract class SettingSerializerDescriptor
    {
        public readonly Type Type;

        protected SettingSerializerDescriptor(Type type)
        {
            this.Type = type;
        }

        public static SettingSerializerDescriptor Create<T>(
            Func<T, string> serialize,
            Func<string, T> deserialize)
        {
            return new SettingSerializerDescriptor<T>(serialize, deserialize);
        }
    }

    public sealed class SettingSerializerDescriptor<T> : SettingSerializerDescriptor
    {
        public readonly Func<T, string> Serialize;
        public readonly Func<string, T> Deserialize;

        public SettingSerializerDescriptor(
            Func<T, string> serialize, 
            Func<string, T> deserialize) : base(typeof(T))
        {
            this.Serialize = serialize;
            this.Deserialize = deserialize;
        }
    }
}
