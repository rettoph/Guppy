using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.SettingSerializers
{
    internal sealed class TimeSpanSerializer : ISettingSerializer<TimeSpan>
    {
        public Type Type { get; } = typeof(TimeSpan);

        public TimeSpan Deserialize(string serialized)
        {
            var milliseconds = int.Parse(serialized);
            var timespan = TimeSpan.FromMilliseconds(milliseconds);

            return timespan;
        }

        public string Serialize(TimeSpan value)
        {
            return value.Milliseconds.ToString();
        }
    }
}
