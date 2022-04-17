using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Settings.Definitions.SettingSerializers
{
    internal sealed class RuntimeSettingSerializerDefinition<T> : SettingSerializerDefinition
    {
        public override Type Type => typeof(T);
        public Func<T, string> Serialize { get; set; }
        public Func<string, T> Deserialize { get; set; }

        public RuntimeSettingSerializerDefinition(Func<T, string> serialize, Func<string, T> deserialize)
        {
            this.Serialize = serialize;
            this.Deserialize = deserialize;
        }

        public override SettingSerializer BuildSerializer()
        {
            return new SettingSerializer<T>(this.Serialize, this.Deserialize);
        }
    }
}
