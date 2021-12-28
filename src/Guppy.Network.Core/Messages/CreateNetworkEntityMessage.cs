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

        #region Read/Write/Filter Methods
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

        /// <summary>
        /// Simple method used to determin whether or not 
        /// a CreateNetworkEntityMessage should be processed
        /// within the peer.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static bool Filter(ServiceProvider p, NetworkMessageConfiguration c)
        {
            if(p.GetService<Peer>() is not ClientPeer)
            {
                return false;
            }

            if(p.GetService<Scene>() is null)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
