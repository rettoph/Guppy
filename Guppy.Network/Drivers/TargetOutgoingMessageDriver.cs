using Guppy.Attributes;
using Guppy.Implementations;
using Guppy.Network.Configurations;
using Guppy.Network.Implementations;
using Guppy.Network.Peers;
using Guppy.Utilities.Pools;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Drivers
{
    /// <summary>
    /// Simple driver used to enqueue created messages
    /// and send them all in bulk based on a designated
    /// send timer.
    /// 
    /// Once a message configuration has been released this
    /// will put it back into the pool so it can be reused.
    /// </summary>
    [IsDriver(typeof(Target))]
    public class TargetOutgoingMessageDriver : Driver<Target>
    {
        #region Private Fields
        private NetOutgoingMessageConfiguration _om;
        private Queue<NetOutgoingMessageConfiguration> _messageQueue;
        private Pool<NetOutgoingMessageConfiguration> _outgoingMessageConfigurationPool;
        #endregion

        #region Constructor
        public TargetOutgoingMessageDriver(Pool<NetOutgoingMessageConfiguration> outgoingMessageConfigurationPool, Target parent) : base(parent)
        {
            _outgoingMessageConfigurationPool = outgoingMessageConfigurationPool;
        }
        #endregion

        #region Lifecycle Methods 
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            _messageQueue = new Queue<NetOutgoingMessageConfiguration>();

            this.UpdateOrder = 110;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.parent.Events.AddDelegate<NetOutgoingMessageConfiguration>("created:message", this.HandleMessageCreated);
        }

        public override void Dispose()
        {
            base.Dispose();

            _messageQueue.Clear();
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Flush the messages
            while(_messageQueue.Count > 0)
            {
                _om = _messageQueue.Dequeue();

                // Ask the parent to send the message...
                this.parent.SendMessage(_om);

                // Add the message back into the pool
                _outgoingMessageConfigurationPool.Put(_om);
            }
        }
        #endregion

        #region Event Handlers
        private void HandleMessageCreated(object sender, NetOutgoingMessageConfiguration arg)
        {
            // When this drivers parent creates a new message we must enqueue it
            _messageQueue.Enqueue(arg);
        }
        #endregion
    }
}
