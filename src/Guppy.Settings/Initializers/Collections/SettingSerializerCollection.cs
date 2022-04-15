using Guppy.Settings.Loaders.Collections;
using Guppy.Settings.Loaders.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Settings.Initializers.Collections
{
    internal sealed class SettingSerializerCollection : List<SettingSerializerDescriptor>, ISettingSerializerCollection
    {
        public SettingSerializerCollection(IEnumerable<SettingSerializerDescriptor> collection) : base(collection)
        {
        }

        public ISettingSerializerCollection Add<T>(Func<T, string> serialize, Func<string, T> deserialize)
        {
            this.Add(SettingSerializerDescriptor.Create<T>(serialize, deserialize));

            return this;
        }
    }
}
