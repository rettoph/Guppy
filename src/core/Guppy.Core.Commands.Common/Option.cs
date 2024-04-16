using System.Reflection;

namespace Guppy.Core.Commands.Common
{
    public sealed class Option
    {
        public readonly PropertyInfo PropertyInfo;
        public readonly string[] Names;
        public readonly string? Description;
        public readonly bool Required;

        public Option(PropertyInfo propertyInfo, string[] names, string? description, bool required)
        {
            this.PropertyInfo = propertyInfo;
            this.Names = names;
            this.Description = description;
            this.Required = required;
        }
    }
}
