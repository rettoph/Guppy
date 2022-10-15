using Guppy.Common;
using Guppy.ECS.Definitions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Filters
{
    internal sealed class SingletonSystemFilter : IFilter<ISystemDefinition>
    {
        private static HashSet<int> _definitions = new HashSet<int>();

        public static SingletonSystemFilter Instance = new SingletonSystemFilter();

        public bool Invoke(IServiceProvider provider, ISystemDefinition arg)
        {
            var response = _definitions.Add(arg.GetHashCode());

            return response;
        }
    }
}
