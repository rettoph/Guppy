using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Services;
using System.Collections;

namespace Guppy.Core
{
    internal sealed class Filtered<T> : IFiltered<T>
        where T : class
    {
        private readonly ILifetimeScope _scope;
        private readonly IServiceFilterService _filters;
        private readonly Lazy<IEnumerable<T>> _unfiltered;

        private IEnumerable<T>? _instances;

        public Filtered(
            ILifetimeScope scope,
            IServiceFilterService filters,
            Lazy<IEnumerable<T>> unfiltered)
        {
            _scope = scope;
            _filters = filters;
            _unfiltered = unfiltered;
        }

        private IEnumerable<T> GetInstances()
        {
            return _unfiltered.Value.Where(x => _filters.Filter(_scope, x)).ToArray();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (_instances ??= this.GetInstances()).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
