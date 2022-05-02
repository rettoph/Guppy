using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaColor = Microsoft.Xna.Framework.Color;

namespace Guppy.Gaming.Definitions
{
    public abstract class ColorDefinition
    {
        public abstract string Key { get; }
        public abstract XnaColor DefaultValue { get; }

        public virtual Color BuildColor()
        {
            return new Color(this.Key, this.DefaultValue);
        }
    }
}
