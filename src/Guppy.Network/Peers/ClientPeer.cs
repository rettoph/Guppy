using Guppy.Network.Enums;
using Guppy.Network.Providers;
using Guppy.Network.Security;
using Guppy.Network.Security.Messages;
using Guppy.Network.Security.Services;
using Guppy.Network.Security.Structs;
using Guppy.Settings.Providers;
using Guppy.Threading;
using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Peers
{
    public sealed class ClientPeer : Peer, ISubscriber<NetIncomingMessage<ConnectionResponseMessage>>
    {
        private NetManager _manager;
        private EventBasedNetListener _listener;
        private NetPeer? _peer;
        private Claim[]? _initialClaims;
        private Bus _bus;

        public ClientPeer(
            IRoomProvider rooms,
            INetMessengerProvider messengers,
            IUserService users,
            ISettingProvider settings,
            EventBasedNetListener listener,
            NetManager manager,
            Bus bus) : base(rooms, messengers, users, settings, listener, manager, bus)
        {
            _manager = manager;
            _listener = listener;
            _bus = bus;

            _bus.Subscribe<NetIncomingMessage<ConnectionResponseMessage>>(this);

            settings.Get<NetworkAuthorization>().Value = NetworkAuthorization.Slave;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public void Start()
        {
            _manager.Start();
        }

        public void Connect(string address, int port, params Claim[] claims)
        {
            var writer = new NetDataWriter();
            var data = new ConnectionRequestMessage(claims);

            ConnectionRequestMessage.Serialize(writer, in data);

            _peer = _manager.Connect(address, port, writer);
            _initialClaims = claims;
        }

        public bool Process(in NetIncomingMessage<ConnectionResponseMessage> message)
        {
            this.CurrentUser = this.Users.UpdateOrCreate(
                message.Content.Instance.Id, 
                _initialClaims ?? message.Content.Instance.Claims, 
                _peer);

            this.CurrentUser.SetClaims(message.Content.Instance.Claims);

            return true;
        }
    }
}
