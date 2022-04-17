using Guppy.Settings.Loaders.Definitions;
using Guppy.Settings.Loaders.Descriptors;

namespace Guppy.Settings.Loaders.Collections
{
    public interface ISettingCollection : IList<SettingDescriptor>
    {
        ISettingCollection Add<T>(string key, string name, string description, T defaultValue, bool exportable, params string[] tags);
        ISettingCollection Add<T>(string name, string description, T defaultValue, bool exportable, params string[] tags);
        ISettingCollection Add<TDefinition>()
            where TDefinition : SettingDefinition;
    }
}
