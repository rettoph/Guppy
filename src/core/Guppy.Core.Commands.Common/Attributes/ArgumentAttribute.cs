using Guppy.Core.Commands.Common.Extensions;
using System.Reflection;

namespace Guppy.Core.Commands.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ArgumentAttribute : FactoryAttribute<Argument>
    {
        public readonly string? Name;
        public readonly string? Description;

        public ArgumentAttribute(string? name = null, string? description = null)
        {
            this.Name = name;
            this.Description = description;
        }

        protected override Argument Build(MemberInfo member)
        {
            Argument option = new Argument(
                propertyInfo: (PropertyInfo)member!,
                name: this.Name ?? "--" + member.Name.LowerCaseFirstLetter(),
                description: this.Description);

            return option;
        }
    }
}
