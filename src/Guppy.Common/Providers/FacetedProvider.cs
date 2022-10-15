using Guppy.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Providers
{
    /// <summary>
    /// Some services are what im calling "faceted".
    /// That is, multiple types can be pulled by more than
    /// just one type value. These type values can overlap
    /// and will not return any results until one specific
    /// implementation type is activated & requested.
    /// </summary>
    public sealed class FacetedProvider<T>
    {
        private IServiceProvider _provider;

        public FacetedStatus Status { get; private set; }

        public Type? Type;
        public T? Instance;

        public FacetedProvider(IServiceProvider provider)
        {
            _provider = provider;
        }

        public TImplementation Activate<TImplementation>(Func<IServiceProvider, TImplementation> factory)
            where TImplementation : T
        {
            if (Status != FacetedStatus.Inactive)
            {
                throw new InvalidOperationException();
            }

            Status = FacetedStatus.Activating;
            Type = typeof(TImplementation);
            Instance = factory(_provider);
            Status = FacetedStatus.Activated;

            return Get<TImplementation>();
        }

        public TService Get<TService>()
            where TService : T
        {
            if (Instance is TService service)
            {
                return service;
            }

            throw new InvalidOperationException();
        }
    }
}
