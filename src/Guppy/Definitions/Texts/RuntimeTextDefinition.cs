using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Definitions.Texts
{
    internal sealed class RuntimeTextDefinition : TextDefinition
    {
        public override string Key { get; }

        public override string? DefaultValue { get; }


        public RuntimeTextDefinition(string key, string? defaultValue)
        {
            this.Key = key;
            this.DefaultValue = defaultValue;
        }
    }
}
