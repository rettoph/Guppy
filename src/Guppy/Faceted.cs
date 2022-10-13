using Guppy.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    /// <summary>
    /// Some services are what im calling "faceted".
    /// That is, multiple types can be pulled by more than
    /// just one type value. These type values can overlap
    /// and will not return any results until one specific
    /// implementation type is activated & requested.
    /// </summary>
    public sealed class Faceted<T>
    {
        private IServiceProvider _provider;

        public FacetedStatus Status { get; private set; }

        public Type? Type;
        public T? Instance;

        public Faceted(IServiceProvider provider)
        {
            _provider = provider;
        }

        public TImplementation Activate<TImplementation>(Func<IServiceProvider, TImplementation> factory)
            where TImplementation : T
        {
            if (this.Status != FacetedStatus.Inactive)
            {
                throw new InvalidOperationException();
            }

            this.Status = FacetedStatus.Activating;
            this.Type = typeof(TImplementation);
            this.Instance = factory(_provider);
            this.Status = FacetedStatus.Activated;

            return this.Get<TImplementation>();
        }

        public TService Get<TService>()
            where TService : T
        {
            if (this.Instance is TService service)
            {
                return service;
            }

            throw new InvalidOperationException();
        }
    }
}
