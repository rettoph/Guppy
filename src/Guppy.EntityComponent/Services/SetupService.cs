using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Services
{
    internal sealed class SetupService : ISetupService
    {
        private Dictionary<Type, ISetup[]> _setups;

        internal SetupService(Dictionary<Type, ISetup[]> setups)
        {
            _setups = setups;
        }

        public bool TryCreate(IEntity entity)
        {
            try
            {
                bool result = true;

                foreach (ISetup setup in _setups[entity.GetType()])
                {
                    result &= setup.TryCreate(entity);
                }

                return result;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool TryDestroy(IEntity entity)
        {
            try
            {
                bool result = true;

                foreach (ISetup setup in _setups[entity.GetType()])
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
