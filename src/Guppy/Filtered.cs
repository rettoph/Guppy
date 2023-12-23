using Guppy.Common;
using Guppy.Common.Providers;

namespace Guppy
{
    internal sealed class Filtered<T> : IFiltered<T>
        where T : class
    {
        private readonly IStateProvider _state;
        private readonly IServiceFilterProvider _filters;
        private readonly Lazy<IEnumerable<T>> _unfiltered;

        private T? _instance;
        private T[]? _instances;

        public T Instance => _instance ??= this.GetInstance();

        public IEnumerable<T> Instances => _instances ??= this.GetInstances();

        public Filtered(
            IStateProvider state,
            IServiceFilterProvider filters,
            Lazy<IEnumerable<T>> unfiltered)
        {
            _state = state;
            _filters = filters;
            _unfiltered = unfiltered;
        }

        private T GetInstance()
        {
            return _unfiltered.Value.First(x => _filters.Filter(_state, x));
        }

        private T[] GetInstances()
        {
            return _unfiltered.Value.Where(x => _filters.Filter(_state, x)).ToArray();
        }
    }
}
