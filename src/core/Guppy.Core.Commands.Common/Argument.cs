using System.Reflection;

namespace Guppy.Core.Commands.Common
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
