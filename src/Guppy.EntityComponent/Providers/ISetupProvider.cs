using Guppy.EntityComponent.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Providers
{
    public interface ISetupProvider
    {
        /// <summary>
        /// Invoke <see cref="Setup.Load"/> on all
        /// internal <see cref="Setup"/> instances.
        /// </summary>
        void Load();

        /// <summary>
        /// Initialize a specific entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool TryInitialize(IEntity entity);

        /// <summary>
        /// Uninitialize a specific entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool TryUninitialize(IEntity entity);
    }
}
