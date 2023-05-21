using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Commands
{
    public sealed class Option
    {
        public readonly PropertyInfo PropertyInfo;
        public readonly string Name;
        public readonly string? Description;
        public readonly bool Required;

        public Option(PropertyInfo propertyInfo, string name, string? description, bool required)
        {
            this.PropertyInfo = propertyInfo;
            this.Name = name;
            this.Description = description;
            this.Required = required;
        }
    }
}
