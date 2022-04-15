using Guppy.Settings.Loaders.Collections;
using Guppy.Settings.Loaders.Descriptors;
using Guppy.Settings.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Settings.Initializers.Collections
{
    internal sealed class SettingCollection : List<SettingDescriptor>, ISettingCollection
    {
        public SettingCollection(IEnumerable<SettingDescriptor> collection) : base(collection)
        {
        }

        public ISettingCollection Add<T>(string key, string name, string description, T defaultValue, bool exportable, params string[] tags)
        {
            this.Add(SettingDescriptor.Create<T>(key, name, description, defaultValue, exportable, tags));

            return this;
        }

        public ISettingCollection Add<T>(string name, string description, T defaultValue, bool exportable, params string[] tags)
        {
            this.Add(SettingDescriptor.Create<T>(name, description, defaultValue, exportable, tags));

            return this;
        }

        public ISettingProvider BuildSettingProvider(ISettingSerializerCollection serializers)
        {
            return new SettingProvider(this.Select(x => x.BuildSetting(serializers)));
        }
    }
}
