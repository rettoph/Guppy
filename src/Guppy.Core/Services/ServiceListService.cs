using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Lists.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Services
{
    /// <summary>
    /// Internal service used to manage all IServiceList instances.
    /// This will automatically maintain the list and is responsible
    /// for auto filling service list instances as needed.
    /// </summary>
    internal class ServiceListService : Service
    {
        #region Private Fields
        private Dictionary<ServiceProvider, List<IServiceList>> _lists;
        #endregion

        #region Lifecycle Methods
        protected override void Create(ServiceProvider provider)
        {
            base.Create(provider);

            _lists = new Dictionary<ServiceProvider, List<IServiceList>>();
        }
        #endregion

        #region Helper Methods
        internal void TryAddList(IServiceList list)
        {
            try
            {
                _lists[list.Provider].Add(list);
                list.OnReleased += this.HandleListReleased;
            }
            catch (KeyNotFoundException e)
            {
                _lists[list.Provider] = new List<IServiceList>();
                this.TryAddList(list);
            }
        }

        internal void TryRemoveList(IServiceList list)
        {
            _lists[list.Provider].Remove(list);
            list.OnReleased -= this.HandleListReleased;
        }

        internal void TryAddService(IService service, ServiceProvider provider)
        {
            try
            {
                _lists[provider].ForEach(l =>
                {
                    if (l.AutoFill && l.BaseType.IsAssignableFrom(service.GetType()))
                        l.TryAdd(service);
                });
            }
            catch(KeyNotFoundException e)
            {
                _lists[provider] = new List<IServiceList>();
                this.TryAddService(service, provider);
            }

            // Recersively add the service to all provider service lists...
            if (provider.root != provider)
                this.TryAddService(service, provider.root);
        }
        #endregion

        #region Event Handlers
        private void HandleListReleased(IService sender)
            => this.TryRemoveList(sender as IServiceList);
        #endregion
    }
}
