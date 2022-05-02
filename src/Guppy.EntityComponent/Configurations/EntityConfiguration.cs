using Guppy.EntityComponent;
using Guppy.EntityComponent.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Configurations
{
    public sealed class EntityConfiguration
    {
        public readonly int Id;
        public readonly Type Type;
        public readonly SetupDefinition[] Creators;
        public readonly SetupDefinition[] Destroyers;

        public EntityConfiguration(int id, Type type, SetupDefinition[] creators, SetupDefinition[] destroyers)
        {
            Id = id;
            Type = type;
            Creators = creators;
            Destroyers = destroyers;
        }
    }
}
