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

        public void Initialize()
        {
            foreach(Setup setup in _setups.Values.SelectMany(x => x).Distinct())
            {
                setup.Initialize(_provider);
            }
        }

        public bool TryCreate(IEntity entity)
        {
            bool result = true;

            foreach (Setup setup in _setups[entity.GetType()])
            {
                result &= setup.TryCreate(entity);
            }

            return result;
        }

        public bool TryDestroy(IEntity entity)
        {
            try
            {
                bool result = true;

                foreach (Setup setup in _setups[entity.GetType()])
                {
                    result &= setup.TryDestroy(entity);
                }

                return result;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
