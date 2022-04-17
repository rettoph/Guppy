using Guppy.EntityComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Loaders.Descriptors
{
    public sealed class SetupDescriptor
    {
        public readonly Type EntityType;
        public readonly Type SetupType;
        public readonly int Order;
        public readonly Func<IServiceProvider, ISetup> Factory;

        private SetupDescriptor(
            Type entityType, 
            Type setupType,
            Func<IServiceProvider, ISetup> factory, 
            int order)
        {
            this.EntityType = entityType;
            this.SetupType = setupType;
            this.Factory = factory;
            this.Order = order;
        }

        public static SetupDescriptor Create<TEntity, TSetup>(
            Func<IServiceProvider, TSetup> factory,
            int order)
                where TEntity : class, IEntity
                where TSetup : class, ISetup
        {
            return new SetupDescriptor(
                typeof(TEntity),
                typeof(TSetup),
                p => factory(p),
                order);
        }

        public static SetupDescriptor Create(
            Type entityType,
            Type setupType,
            Func<IServiceProvider, ISetup> factory,
            int order)
        {
            typeof(IEntity).ValidateAssignableFrom(entityType);
            typeof(ISetup).ValidateAssignableFrom(setupType);

            return new SetupDescriptor(
                entityType,
                setupType,
                factory,
                order);
        }
    }
}
