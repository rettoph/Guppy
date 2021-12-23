using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.CommandLine.Arguments
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class CommandParentAttribute : Attribute
    {
        public readonly Type Type;

        public CommandParentAttribute(Type type)
        {
            typeof(CommandDefinition).ValidateAssignableFrom(type);

            this.Type = type;
        }
    }
}
