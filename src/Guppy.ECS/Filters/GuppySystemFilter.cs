using Guppy.Common;
using Guppy.Common.Providers;
using Guppy.ECS.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Filters
{
    internal sealed class GuppySystemFilter<T> : IFilter<ISystemDefinition>
        where T : IGuppy
    {
        private Faceted<IGuppy> _guppy;

        public GuppySystemFilter(Faceted<IGuppy> guppy)
        {
            _guppy = guppy;
        }

        public bool Invoke(ISystemDefinition arg)
        {
            var result = typeof(T).IsAssignableFrom(_guppy.Type);

            return result;
        }
    }
}
