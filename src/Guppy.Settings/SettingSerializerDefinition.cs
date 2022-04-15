using Guppy.Settings.Loaders.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Settings
{
    public abstract class SettingSerializerDefinition
    {
        public abstract Type Type { get; }

        internal SettingSerializerDefinition()
        {

        }

        public abstract SettingSerializerDescriptor BuildDescriptor();
    }

    public abstract class SettingSerializerDefinition<T> : SettingSerializerDefinition
    {
        public override Type Type => typeof(T);

        public abstract string Serialize(T deserialized);
        public abstract T Deserialize(string serialized);

        public override SettingSerializerDescriptor BuildDescriptor()
        {
            return SettingSerializerDescriptor.Create(this.Serialize, this.Deserialize);
        }
    }
}
