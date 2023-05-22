using Guppy.Commands.Attributes;
using Guppy.Commands.Extensions;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Guppy.Commands.Arguments
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
