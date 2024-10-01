using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Services;
using System.Collections;

namespace Guppy.Core
{
    internal sealed class Filtered<T>(
        ILifetimeScope scope,
        IServiceFilterService filters,
        Lazy<IEnumerable<T>> unfiltered) : IFiltered<T>
        where T : class
    {
        private readonly ILifetimeScope _scope = scope;
        private readonly IServiceFilterService _filters = filters;
        private readonly Lazy<IEnumerable<T>> _unfiltered = unfiltered;

        private IEnumerable<T>? _instances;

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
