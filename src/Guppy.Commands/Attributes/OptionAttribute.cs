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
    public class OptionAttribute : FactoryAttribute<Option>
    {
        public readonly string? Name;
        public readonly string? Description;
        public readonly bool Required;

        public OptionAttribute(string? name = null, string? description = null, bool required = false)
        {
            this.Name = name;
            this.Description = description;
            this.Required = required;
        }

        protected override Option Build(MemberInfo member)
        {
            Option option = new Option(
                propertyInfo: (PropertyInfo)member!, 
                name: this.Name ?? member.Name.LowerCaseFirstLetter(), 
                description: this.Description,
                required: this.Required);

            return option;
        }
    }
}
