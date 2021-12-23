using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.CommandLine.Arguments
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class CommandInfoAttribute : Attribute
    {
        public readonly String Name;
        public readonly String Description;
        public readonly String[] Aliases;

        public CommandInfoAttribute(String name, String description, params String[] aliases)
        {
            this.Name = name;
            this.Description = description;
            this.Aliases = aliases;
        }
    }
}
