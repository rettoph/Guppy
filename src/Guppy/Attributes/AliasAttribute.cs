using Guppy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Attributes
{
    public sealed class AliasAttribute : Attribute
    {
        public readonly Type Alias;
        public readonly Type? Implementation;

        public AliasAttribute(Type alias, Type? implementation = null)
        {
            if(implementation is not null)
            {
                ThrowIf.Type.IsNotAssignableFrom(alias, implementation);
            }

            this.Alias = alias;
            this.Implementation = implementation;
        }
    }
}
