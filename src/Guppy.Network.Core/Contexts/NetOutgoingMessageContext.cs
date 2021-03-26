using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Interfaces;

namespace Guppy.Network.Contexts
{
    /// <summary>
    /// Defines simple reusable values for <see cref="NetOutgoingMessage"/>s.
    /// <see cref="IPipe.CreateMessage"/> consumes this value.
    /// </summary>
    public class NetOutgoingMessageContext
    {
        #region Public Fields
        public Int32 SequenceChannel;
        public NetDeliveryMethod Method;
        #endregion
    }
}
