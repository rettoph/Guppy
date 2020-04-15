using Guppy.DependencyInjection;
using Guppy.Network.Utilities;
using Lidgren.Network;
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
    public class Messageable : Service
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

            this.MessageDelegater = new MessageDelegater();
        }

        protected override void Dispose()
        {
            base.Dispose();
        }
        #endregion

        #region Frame Methods
        public virtual void Update()
        {
            while (this.Messages.Any())
                this.MessageDelegater.Handle(this, this.Messages.Dequeue());
        }
        #endregion
    }
}
