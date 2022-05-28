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

        private readonly Func<TEntity, bool> _initialize;
        private readonly Func<TEntity, bool> _uninitialize;


        public RuntimeSetupDefinition(
            Func<TEntity, bool> initialize,
            Func<TEntity, bool> uninitialize,
            int order)
        {
            _initialize = initialize;
            _uninitialize = uninitialize;

            this.Order = order;
        }

        protected override bool TryInitialize(IEntity entity)
        {
            return entity is TEntity casted && _initialize(casted);
        }

        protected override bool TryUninitialize(IEntity entity)
        {
            return entity is TEntity casted && _uninitialize(casted);
        }
    }
}
