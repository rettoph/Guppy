using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Resources.Serialization.Json;

namespace Guppy.Resources.Definitions
{
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
