using Guppy.Interfaces;
using Guppy.Lists;
using Guppy.Network.Contexts;
using Guppy.Network.Lists;
using Guppy.Network.Security;
using Guppy.Network.Utilities;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Interfaces
{
    public interface IPipe : IEntity, IEnumerable<IUser>
    {
        #region Properties
        /// <summary>
        /// The owning <see cref="IChannel"/>.
        /// </summary>
        IChannel Channel { get; internal set; }

        UserList Users { get; }
        IEnumerable<INetworkEntity> NetworkEntities { get; }
        #endregion

        #region Events
        public delegate void NetworkEnityAddedToPipeDelegate(IPipe sender, INetworkEntity networkEntity, IPipe oldPipe);

        event NetworkEnityAddedToPipeDelegate OnNetworkEnityAddedToPipe;
        #endregion

        #region Methods
        internal void TryAdd(INetworkEntity entity, IPipe oldPipe);
        internal void TryRemove(INetworkEntity entity);
        #endregion
    }
}
