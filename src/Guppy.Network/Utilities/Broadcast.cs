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
        #region Classes
        private class Broadcaster
        {
            public Guid Id;
            public INetworkEntity Entity;
            public Action<NetOutgoingMessage> Cleaner;

            public Boolean ShouldBroadcast()
                => this.Entity.Status == ServiceStatus.Ready && this.Id == this.Entity.Id;

            public Boolean TryBroadcast(UInt32 messageType)
            {
                if (this.ShouldBroadcast())
                {
                    this.Entity.Messages[messageType].Create(this.Cleaner, this.Entity.Pipe);

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region Static Fields
        private static Queue<Broadcaster> BroadcastPool = new Queue<Broadcaster>();
        #endregion

        #region Private Fields
        private Queue<Broadcaster> _dirtyBroadcasters;
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

            _dirtyBroadcasters = new Queue<Broadcaster>();

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
            _dirtyBroadcasters.Enqueue(Broadcast.GetBroadcaster(dirtyEntity, cleaner));
        }

        private void Flush(GameTime gameTime)
        {
            while(_dirtyBroadcasters.Any())
            {
                Broadcaster dirtyBroadcaster = _dirtyBroadcasters.Dequeue();

                if(!dirtyBroadcaster.TryBroadcast(this.MessageType))
                {
                    this.log.Warn($"Attempted to broadcast message for irrelevant entity.");
                }

                Broadcast.BroadcastPool.Enqueue(dirtyBroadcaster);
            }
        }
        #endregion

        #region Static Methods
        private static Broadcaster GetBroadcaster(INetworkEntity entity, Action<NetOutgoingMessage> cleaner)
        {
            if(Broadcast.BroadcastPool.Any())
            {
                Broadcaster broadcaster = Broadcast.BroadcastPool.Dequeue();
                broadcaster.Id = entity.Id;
                broadcaster.Entity = entity;
                broadcaster.Cleaner = cleaner;

                return broadcaster;
            }

            return new Broadcaster()
            {
                Id = entity.Id,
                Entity = entity,
                Cleaner = cleaner
            };
        }
        #endregion
    }
}
