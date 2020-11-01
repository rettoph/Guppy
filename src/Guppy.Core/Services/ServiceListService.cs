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
        private Dictionary<GuppyServiceProvider, List<IServiceList>> _autoFillLists;
        #endregion

        #region Lifecycle Methods
        protected override void Create(GuppyServiceProvider provider)
        {
            base.Create(provider);

            _autoFillLists = new Dictionary<GuppyServiceProvider, List<IServiceList>>();
        }
        #endregion

        #region Helper Methods
        internal void TryAddList(IServiceList list)
        {
            if(!_autoFillLists.ContainsKey(list.Provider))
                _autoFillLists[list.Provider] = new List<IServiceList>();

            if(list.AutoFill)
            { // Track the list if needed...
                _autoFillLists[list.Provider].Add(list);
                list.OnReleased += this.HandleListReleased;
            }
        }

        internal void TryRemoveList(IServiceList list)
        {
            _autoFillLists[list.Provider].Remove(list);
            list.OnReleased -= this.HandleListReleased;
        }

        internal void TryAddService(IService service, GuppyServiceProvider provider)
        {
            try
            {
                _autoFillLists[provider].ForEach(l =>
                {
                    if (l.BaseType.IsAssignableFrom(service.GetType()))
                        l.TryAdd(service);
                });
            }
            catch(KeyNotFoundException e)
            {
                _autoFillLists[provider] = new List<IServiceList>();
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
