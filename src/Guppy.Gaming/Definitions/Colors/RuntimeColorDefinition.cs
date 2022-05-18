using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaColor = Microsoft.Xna.Framework.Color;

namespace Guppy.Gaming.Definitions.Colors
{
    internal sealed class RuntimeColorDefinition : ColorDefinition
    {
        public override string Key { get; }

        public override XnaColor DefaultValue { get; }

        public RuntimeColorDefinition(string name, XnaColor defaultValue)
        {
            this.Key = name;
            this.DefaultValue = defaultValue;
        }
    }
}
