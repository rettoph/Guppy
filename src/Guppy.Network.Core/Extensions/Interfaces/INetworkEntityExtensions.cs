using Guppy.Network.Interfaces;
using Guppy.Network.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public static class INetworkEntityExtensions
    {
        /// <summary>
        /// Populate the recieved <typeparamref name="TNetworkEntityMessage"/>'s
        /// <see cref="NetworkEntityMessage.NetworkId"/> and 
        /// <see cref="NetworkEntityMessage.Packets"/>.
        /// </summary>
        /// <typeparam name="TNetworkEntityMessage"></typeparam>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        public static void PopulateMessage<TNetworkEntityMessage>(this INetworkEntity entity, TNetworkEntityMessage message)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>
        {
            message.NetworkId = entity.NetworkId;
            message.Packets.AddRange(entity.Messages.GetAll<TNetworkEntityMessage>());
        }
    }
}
