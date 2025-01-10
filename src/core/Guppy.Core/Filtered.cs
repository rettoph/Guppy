using System.Collections;
using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Services;

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

        private T[] GetInstances()
        {
            return this._unfiltered.Value.Where(x => this._filters.Filter(this._scope, x)).ToArray();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (this._instances ??= this.GetInstances()).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}