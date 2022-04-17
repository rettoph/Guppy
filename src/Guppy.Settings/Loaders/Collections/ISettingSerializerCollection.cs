using Guppy.Settings.Loaders.Definitions;
using Guppy.Settings.Loaders.Descriptors;

namespace Guppy.Settings.Loaders.Collections
{
    public interface ISettingSerializerCollection : IList<SettingSerializerDescriptor>
    {
        ISettingSerializerCollection Add<T>(Func<T, string> serialize, Func<string, T> deserialize);
        ISettingSerializerCollection Add<TDefinition>()
            where TDefinition : SettingSerializerDefinition;
    }
}
