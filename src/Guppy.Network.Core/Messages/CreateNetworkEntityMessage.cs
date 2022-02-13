using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Configurations;
using Guppy.Network.Interfaces;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Messages
{
    public sealed class CreateNetworkEntityMessage : NetworkEntityMessage<CreateNetworkEntityMessage>
    {
        public UInt32 ServiceConfigurationId { get; internal set; }

        #region Read/Write Methods
        protected internal override void Read(NetDataReader im, NetworkProvider network)
        {
            base.Read(im, network);

            this.ServiceConfigurationId = im.GetUInt();
        }

        protected internal override void Write(NetDataWriter om, NetworkProvider network)
        {
            base.Write(om, network);

            om.Put(this.ServiceConfigurationId);
        }
        #endregion
    }
}
