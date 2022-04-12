using Guppy.EntityComponent;
using Guppy.EntityComponent.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent
{
    public abstract class Entity : IEntity
    {
        Type IEntity.Type => this.Type;

        protected virtual Type Type => this.GetType();

        public readonly Guid Id;

        public IComponentService Components { get; set; }

        public Entity()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
