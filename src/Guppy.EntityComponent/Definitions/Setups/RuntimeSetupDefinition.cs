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

        private readonly Func<IServiceProvider, TEntity, bool> _create;
        private readonly Func<IServiceProvider, TEntity, bool> _destroy;


        public RuntimeSetupDefinition(
            Func<IServiceProvider, TEntity, bool> create,
            Func<IServiceProvider, TEntity, bool> destroy,
            int order)
        {
            _create = create;
            _destroy = destroy;

            this.Order = order;
        }

        protected override bool TryCreate(IServiceProvider provider, IEntity entity)
        {
            return entity is TEntity casted && _create(provider, casted);
        }

        protected override bool TryDestroy(IServiceProvider provider, IEntity entity)
        {
            return entity is TEntity casted && _destroy(provider, casted);
        }
    }
}
