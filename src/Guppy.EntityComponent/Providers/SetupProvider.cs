using Guppy.EntityComponent.Definitions;
using Guppy.EntityComponent.Services;
using Minnow.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Providers
{
    internal sealed class SetupProvider : ISetupProvider
    {
        private Dictionary<Type, Setup[]> _setups;
        private readonly IServiceProvider _provider;

        public SetupProvider(IServiceProvider provider, ITypeProvider<IEntity> entities, IEnumerable<SetupDefinition> definitions)
        {
            _provider = provider;

            var setups = definitions.Select(x => x.BuildSetup()).OrderBy(x => x.Order).ToArray();

            _setups = new Dictionary<Type, Setup[]>(entities.Count());
            foreach(Type entity in entities)
            {
                _setups.Add(entity, setups.Where(s => s.EntityType.IsAssignableFrom(entity)).ToArray());
            }
        }

        public void Load()
        {
            foreach(Setup setup in _setups.Values.SelectMany(x => x).Distinct())
            {
                setup.Load(_provider);
            }
        }

        public bool TryInitialize(IEntity entity)
        {
            bool result = true;

            foreach (Setup setup in _setups[entity.GetType()])
            {
                result &= setup.TryInitialize(entity);
            }

            return result;
        }

        public bool TryUninitialize(IEntity entity)
        {
            bool result = true;

            foreach (Setup setup in _setups[entity.GetType()])
            {
                result &= setup.TryUninitialize(entity);
            }

            return result;
        }
    }
}
