using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Providers
{
    /// <summary>
    /// Some service's are what im calling "activated".
    /// That is, multiple types can be pulled by more than
    /// just one type value. These type values can overlap
    /// and will not return any results until one specific
    /// implementation type is requested.
    /// </summary>
    internal sealed class ActivatedServiceProvider<T>
    {
        private T? _instance;
        private IServiceProvider _provider;

        public T Instance => _instance ?? throw new NullReferenceException($"{nameof(ActivatedServiceProvider<T>)}::{nameof(Instance)}");

        public ActivatedServiceProvider(IServiceProvider provider)
        {
            _provider = provider;
        }

        public TImplementation TryActivate<TImplementation>(Func<IServiceProvider, TImplementation> factory)
            where TImplementation : T
        {
            _instance ??= factory(_provider);

            return this.Get<TImplementation>();
        }

        public TService Get<TService>()
            where TService : T
        {
            if(_instance is TService service)
            {
                return service;
            }

            throw new InvalidOperationException();
        }
    }
}
