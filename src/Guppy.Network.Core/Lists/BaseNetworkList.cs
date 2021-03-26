using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Lists;
using Guppy.Lists.Delegates;
using Guppy.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Lists
{
    public class BaseNetworkList<TService> : ServiceList<TService>
        where TService : class, IService
    {
        #region Protected Properties
        protected internal UInt32 serviceConfigurationId { get; internal set; }
        #endregion

        #region Events
        public event ItemDelegate<TService> OnCreated;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.serviceConfigurationId = ServiceConfiguration.GetId<TService>();
        }
        #endregion

        #region Create Methods
        protected virtual T Create<T>(ServiceProvider provider, Action<T, ServiceProvider, ServiceConfiguration> setup = null, Guid? id = null)
            where T : class, TService
        {
            var instance = provider.GetService<T>(this.serviceConfigurationId, (i, p, d) =>
            {
                if (id != null)
                    i.Id = id.Value;

                this.OnCreated?.Invoke(i);

                setup?.Invoke(i, p, d);
            });

            this.TryAdd(instance);

            return instance;
        }
        #endregion
    }
}
