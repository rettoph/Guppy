using Guppy;
using Guppy.DependencyInjection;
using Guppy.Enums;
using Guppy.Network.Interfaces;
using Guppy.Utilities;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Network.Utilities
{
    /// <summary>
    /// Represents a single registered boradcast type.
    /// To create one just call <see cref="BroadcastService.Register"/>
    /// </summary>
    public sealed class Broadcast : Service
    {
        #region Private Fields
        private Queue<INetworkEntity> _dirtyEntities;
        private Queue<Action<NetOutgoingMessage>> _cleaners;
        private IntervalInvoker _intervals;
        #endregion

        #region Public Properties
        public UInt32 MessageType { get; internal set; }
        public Double Milliseconds { get; internal set; }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _intervals);
        }

        protected override void Initialize(GuppyServiceProvider provider)
        {
            base.Initialize(provider);

            _dirtyEntities = new Queue<INetworkEntity>();
            _cleaners = new Queue<Action<NetOutgoingMessage>>();

            _intervals[this.Milliseconds].OnInterval += this.Flush;
        }

        protected override void Release()
        {
            base.Release();

            _intervals[this.Milliseconds].OnInterval -= this.Flush;
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            _intervals = default;
        }
        #endregion

        #region Frame Methods
        /// <summary>
        /// Enqueue a dirty entity and its cleaner. Next time this
        /// broadcast gets flushed the entity will be cleaned.
        /// </summary>
        /// <param name="dirtyEntity"></param>
        /// <param name="cleaner"></param>
        public void Enqueue(INetworkEntity dirtyEntity, Action<NetOutgoingMessage> cleaner)
        {
            _dirtyEntities.Enqueue(dirtyEntity);
            _cleaners.Enqueue(cleaner);
        }

        private void Flush(GameTime gameTime)
        {
            while(_dirtyEntities.Any())
            {
                INetworkEntity dirtyEntity = _dirtyEntities.Dequeue();
                Action<NetOutgoingMessage> cleaner = _cleaners.Dequeue();

                if(dirtyEntity.Status == ServiceStatus.Ready)
                {
                    NetOutgoingMessage om = dirtyEntity.Messages[this.MessageType].Create(dirtyEntity.Pipe);
                    cleaner(om);
                }
                else
                {
                    this.log.Warn($"Attempted to clean non ready dirty entity.");
                }
            }
        }
        #endregion
    }
}
