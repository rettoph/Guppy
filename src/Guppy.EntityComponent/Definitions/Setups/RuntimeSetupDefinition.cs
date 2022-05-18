using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Definitions.Setups
{
    internal sealed class RuntimeSetupDefinition<TEntity> : SetupDefinition
        where TEntity : class, IEntity
    {
        public override Type EntityType { get; } = typeof(TEntity);

        public override int Order { get; }

        private readonly Func<TEntity, bool> _create;
        private readonly Func<TEntity, bool> _destroy;


        public RuntimeSetupDefinition(
            Func<TEntity, bool> create,
            Func<TEntity, bool> destroy,
            int order)
        {
            _create = create;
            _destroy = destroy;

            this.Order = order;
        }

        protected override bool TryCreate(IEntity entity)
        {
            return entity is TEntity casted && _create(casted);
        }

        protected override bool TryDestroy(IEntity entity)
        {
            return entity is TEntity casted && _destroy(casted);
        }
    }
}
