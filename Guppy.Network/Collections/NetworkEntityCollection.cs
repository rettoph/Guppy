using Guppy.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Collections
{
    public class NetworkEntityCollection : FrameableCollection<NetworkEntity>
    {
        #region Private Fields
        private EntityCollection _entities;
        #endregion

        #region Constructor
        public NetworkEntityCollection(EntityCollection entities, IServiceProvider provider) : base(provider)
        {
            _entities = entities;

            _entities.Events.AddDelegate<Entity>("added", this.HandleEntityAdded);
            _entities.Events.AddDelegate<Entity>("removed", this.HandleEntityRemoved);
        }
        #endregion

        #region Event Handlers
        private void HandleEntityAdded(object sender, Entity arg)
        {
            if (arg is NetworkEntity)
                this.Add(arg as NetworkEntity);
        }

        private void HandleEntityRemoved(object sender, Entity arg)
        {
            if (arg is NetworkEntity)
                this.Remove(arg as NetworkEntity);
        }
        #endregion
    }
}
