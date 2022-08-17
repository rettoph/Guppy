using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Definitions
{
    public sealed class TagDefinition
    {
        public readonly string Name;
        public readonly Type[] Components;

        public TagDefinition(string name, Type[] components)
        {
            this.Name = name;
            this.Components = components;
        }
    }
}
