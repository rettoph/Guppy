using Autofac;
using Guppy.Common;
using Guppy.Common.Services;

namespace Guppy
{
    internal sealed class Filtered<T> : IFiltered<T>
        where T : class
    {
        private readonly ILifetimeScope _scope;
        private readonly IServiceFilterService _filters;
        private readonly Lazy<IEnumerable<T>> _unfiltered;

        private T? _instance;
        private T[]? _instances;

        public T Instance => _instance ??= this.GetInstance();

        public IEnumerable<T> Instances => _instances ??= this.GetInstances();

        public Filtered(
            ILifetimeScope scope,
            IServiceFilterService filters,
            Lazy<IEnumerable<T>> unfiltered)
        {
            _scope = scope;
            _filters = filters;
            _unfiltered = unfiltered;
        }

        private T GetInstance()
        {
            return _unfiltered.Value.First(x => _filters.Filter(_scope, x));
        }

        private T[] GetInstances()
        {
            return _unfiltered.Value.Where(x => _filters.Filter(_scope, x)).ToArray();
        }
    }
}
