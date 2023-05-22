using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Commands
{
    public sealed class Argument
    {
        public readonly PropertyInfo PropertyInfo;
        public readonly string Name;
        public readonly string? Description;

        public Argument(PropertyInfo propertyInfo, string name, string? description)
        {
            this.PropertyInfo = propertyInfo;
            this.Name = name;
            this.Description = description;
        }
    }
}
