using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaColor = Microsoft.Xna.Framework.Color;

namespace Guppy.Gaming
{
    public sealed class Color : Resource<XnaColor>
    {
        public Color(string key, XnaColor defaultValue) : base(key, defaultValue)
        {
        }

        public override string? Export()
        {
            return this.Value.PackedValue.ToString();
        }

        public override void Import(string? value)
        {
            this.Value = new XnaColor(uint.Parse(value ?? "0"));
        }
    }
}
