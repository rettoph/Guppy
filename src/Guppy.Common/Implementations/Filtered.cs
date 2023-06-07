using Guppy.Common;
using Guppy.Common.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Implementations
{
    internal sealed class Filtered<T> : IFiltered<T>
        where T : class
    {
        private readonly IFilterProvider _filters;
        private readonly Lazy<IEnumerable<T>> _unfiltered;

        private T? _instance;
        private T[]? _instances;

        public T Instance => _instance ??= this.GetInstance();

        public IEnumerable<T> Instances => _instances ??= this.GetInstances();

        public Filtered(
            IFilterProvider filters,
            Lazy<IEnumerable<T>> unfiltered)
        {
            _filters = filters;
            _unfiltered = unfiltered;
        }

        private T GetInstance()
        {
            return _unfiltered.Value.First(_filters.Filter);
        }

        private T[] GetInstances()
        {
            return _unfiltered.Value.Where(_filters.Filter).ToArray();
        }
    }
}
