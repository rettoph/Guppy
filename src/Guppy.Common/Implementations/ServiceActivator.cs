using Guppy.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Implementations
{
    /// <summary>
    /// Some services are what im calling "activated".
    /// That is, multiple types can be pulled by more than
    /// just one type value. These type values can overlap
    /// and will not return any results until one specific
    /// implementation type is activated & requested.
    /// </summary>
    public sealed class ServiceActivator<T>
    {
        private IServiceProvider _provider;

        public ActivationStatus Status { get; private set; }

        public Type? Type;
        public T? Instance;

        public ServiceActivator(IServiceProvider provider)
        {
            _provider = provider;
        }

        public TImplementation Activate<TImplementation>(Func<IServiceProvider, TImplementation> factory)
            where TImplementation : T
        {
            if (this.Status != ActivationStatus.Inactive)
            {
                throw new InvalidOperationException();
            }

            this.Status = ActivationStatus.Activating;
            this.Type = typeof(TImplementation);
            this.Instance = factory(_provider);
            this.Status = ActivationStatus.Activated;

            return Get<TImplementation>();
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
