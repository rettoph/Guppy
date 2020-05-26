using Guppy.DependencyInjection;
using Guppy.Network.Utilities;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xxHashSharp;

namespace Guppy.Network
{
    /// <summary>
    /// An Object capable of sending & recieving messages.
    /// </summary>
    public class Messageable : Asyncable
    {
        #region Public Attributes
        /// <summary>
        /// Contains custom delegates to excecute based on message identification.
        /// </summary>
        public MessageDelegater MessageDelegater { get; private set; }

        /// <summary>
        /// Messages to be read next invocation of Update
        /// </summary>
        public Queue<NetIncomingMessage> Messages { get; private set; }
        #endregion

        #region Lifeccyle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Messages = new Queue<NetIncomingMessage>();
            this.MessageDelegater = new MessageDelegater();
        }

        protected override void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            while (this.Messages.Any())
                this.MessageDelegater.Handle(this, this.Messages.Dequeue());
        }
        #endregion
    }
}
