using Guppy.EntityComponent;
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
        public readonly ISetup[] Creators;
        public readonly ISetup[] Destroyers;

        public EntityConfiguration(int id, Type type, ISetup[] creators, ISetup[] destroyers)
        {
            Id = id;
            Type = type;
            Creators = creators;
            Destroyers = destroyers;
        }
    }
}
