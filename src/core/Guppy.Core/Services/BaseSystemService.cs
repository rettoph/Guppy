﻿using Guppy.Core.Common.Providers;
using Guppy.Core.Common.Services;
using Guppy.Core.Common.Systems;

namespace Guppy.Core.Services
{
    public abstract class BaseSystemService<TSystem>(IEnumerable<TSystem> systems, IEnumerable<ISystemProvider<TSystem>> providers) : ISystemService<TSystem>
        where TSystem : ISystem
    {
        private readonly List<TSystem> _systems = [.. systems, .. providers.SelectMany(x => x.GetSystems())];

        public IEnumerable<TSystem> GetAll()
        {
            return this._systems;
        }
    }
}
