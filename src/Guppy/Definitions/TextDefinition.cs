using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Definitions
{
    public abstract class TextDefinition
    {
        public abstract string Key { get; }
        public abstract string? DefaultValue { get; }

        public virtual Text BuildText()
        {
            return new Text(this.Key, this.DefaultValue);
        }
    }
}
