using Guppy.EntityComponent.Loaders.Collections;
using Guppy.EntityComponent.Loaders.Descriptors;
using Guppy.EntityComponent.Providers;
using Microsoft.Extensions.DependencyInjection;
using Minnow.System.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Initializers.Collections
{
    internal sealed class SetupCollection : List<SetupDescriptor>, ISetupCollection
    {
        

        public ISetupCollection Add<TEntity, TSetup>(
            Func<IServiceProvider, TSetup> factory, int order)
                where TEntity : class, IEntity
                where TSetup : class, ISetup
        {
            this.Add(SetupDescriptor.Create<TEntity, TSetup>(factory, order));

            return this;
        }

        public ISetupCollection Add<TEntity, TSetup>(
            int order)
                where TEntity : class, IEntity
                where TSetup : class, ISetup
        {
            this.Add(SetupDescriptor.Create(typeof(TEntity), typeof(TSetup), ActivatorUtilitiesHelper.BuildFactory<TSetup>(), order));

            return this;
        }

        public ISetupProvider BuildProvider(IEnumerable<Type> entities)
        {
            Dictionary<Type, SetupDescriptor[]> configurations = entities.ToDictionary(
                keySelector: t => t,
                elementSelector: t => this.Where(cd => cd.EntityType.IsAssignableFrom(t)).ToArray());

            return new SetupProvider(configurations);
        }
    }
}
