using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.DependencyInjection
{
    public sealed class AliasDescriptor
    {
        public Type Alias { get; }
        public AliasType Type { get; set; }

        public AliasDescriptor(Type alias, AliasType type)
        {
            this.Alias = alias;
            this.Type = type;
        }
    }
}
