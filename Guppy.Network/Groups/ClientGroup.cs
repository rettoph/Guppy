using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Collections;
using Guppy.Network.Configurations;
using Guppy.Utilities.Pools;
using Lidgren.Network;

namespace Guppy.Network.Groups
{
    public class ClientGroup : Group
    {
        #region Private Fields
        private NetClient _client;
        #endregion

        #region Constructor
        public ClientGroup(NetClient client) : base(client)
        {
            _client = client;
        }
        #endregion

        #region Target Implmentation
        public override void SendMessage(NetOutgoingMessageConfiguration om)
        {
            _client.SendMessage(om.Message, om.Method, om.SequenceChannel);  
        }
        #endregion
    }
}
