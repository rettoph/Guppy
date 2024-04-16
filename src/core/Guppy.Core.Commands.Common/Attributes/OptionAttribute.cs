using Guppy.Core.Commands.Common.Extensions;
using System.Reflection;

namespace Guppy.Core.Commands.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionAttribute : FactoryAttribute<Option>
    {
        public readonly string[]? Names;
        public readonly string? Description;
        public readonly bool Required;

        public OptionAttribute(string[]? names = null, string? description = null, bool required = false)
        {
            this.Names = names;
            this.Description = description;
            this.Required = required;
        }

        protected override Option Build(MemberInfo member)
        {
            Option option = new Option(
                propertyInfo: (PropertyInfo)member!,
                names: this.Names ?? new[] { "-" + member.Name.LowerCaseFirstLetter() },
                description: this.Description,
                required: this.Required);

            return option;
        }
    }
}
