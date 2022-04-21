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

        public SetupProvider(ITypeProvider<IEntity> entities, IEnumerable<SetupDefinition> definitions)
        {
            var setups = definitions.Select(x => x.BuildSetup()).ToArray();

            _setups = new Dictionary<Type, Setup[]>(entities.Count());
            foreach(Type entity in entities)
            {
                _setups.Add(entity, setups.Where(s => s.EntityType.IsAssignableFrom(entity)).ToArray());
            }
        }

        public bool TryCreate(IServiceProvider provider, IEntity entity)
        {
            bool result = true;

            foreach (Setup setup in _setups[entity.GetType()])
            {
                result &= setup.TryCreate(provider, entity);
            }

            return result;
        }

        public bool TryDestroy(IServiceProvider provider, IEntity entity)
        {
            try
            {
                bool result = true;

                foreach (Setup setup in _setups[entity.GetType()])
                {
                    result &= setup.TryDestroy(provider, entity);
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
