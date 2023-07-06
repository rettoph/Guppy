using Guppy.Attributes;
using Guppy.Enums;
using Guppy.Resources.Serialization.Json;

namespace Guppy.Resources.Definitions
{
    [Service<ISettingDefinition>(ServiceLifetime.Singleton, true)]
    public interface ISettingDefinition
    {
        string Key { get; }
        bool Exportable { get; }
        string[] Tags { get; }
        Type Type { get; }
        object DefaultValue { get; }

        ISetting Build(IJsonSerializer json);
    }
}
