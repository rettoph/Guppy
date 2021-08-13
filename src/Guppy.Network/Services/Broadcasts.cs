using Guppy;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Utilities;
using Guppy.DependencyInjection;
using Microsoft.Xna.Framework;

namespace Guppy.Network.Services
{
    /// <summary>
    /// The primary broadcast service used to send network entity data 
    /// at an interval. Each boradcast type must be registered in a ServiceLoader
    /// before it can be accessed.
    /// </summary>
    public sealed class Broadcasts : Service
    {
        #region Private Fields
        private Dictionary<UInt32, Broadcast> _broadcasts;
        private GuppyServiceProvider _provider;
        #endregion

        #region Public Properties
        public Broadcast this[UInt32 messageType]
        {
            get => _broadcasts[messageType];
        }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.Initialize(provider);

            _broadcasts = new Dictionary<UInt32, Broadcast>();
            _provider = provider;
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            foreach (Broadcast broadcast in _broadcasts.Values)
                broadcast.TryRelease();

            _broadcasts.Clear();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Register a new broadcast type.
        /// </summary>
        /// <param name="messageType">The message type, matching a network entities <see cref="MessageManager.Add(uint, Guppy.Network.Contexts.NetOutgoingMessageContext, Func{Guppy.Network.Contexts.NetOutgoingMessageContext, IEnumerable{Lidgren.Network.NetConnection}, Lidgren.Network.NetOutgoingMessage})"/> id.</param>
        /// <param name="milliseconds">The number of milliseconds to wait between each broadcast.</param>
        public void Register(UInt32 messageType, Double milliseconds)
        {
            _broadcasts.Add(messageType, _provider.GetService<Broadcast>((broadcast, _, _) =>
            {
                broadcast.MessageType = messageType;
                broadcast.Milliseconds = milliseconds;
            }));
        }
        #endregion
    }
}
