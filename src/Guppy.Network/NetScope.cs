using Guppy.EntityComponent;
using Guppy.EntityComponent.Services;
using Guppy.Network.Constants;
using Guppy.Network.Entities;
using Guppy.Network.Enums;
using Guppy.Network.Providers;
using Guppy.Network.Services;
using Guppy.Providers;
using Guppy.Threading;
using LiteNetLib.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public sealed class NetScope : Entity
    {
        private NetScopeState _state;
        private readonly IServiceProvider _provider;

        internal byte id;

        public new byte Id => this.id;
        public NetScopeState State
        {
            get => _state;
            set => this.OnStateChanged!.InvokeIf(value != _state, this, ref _state, value);
        }

        public readonly NetScopeUserService Users;
        public readonly INetScopeIncomingMessageService Incoming;
        public readonly INetScopeOutgoingMessageService Outgoing;

        public NetSystemMessenger? Messenger { get; private set; }

        public event OnChangedEventDelegate<NetScope, NetScopeState>? OnStateChanged;

        public NetScope(IServiceProvider provider, ISettingProvider settings, INetTargetService targets, INetMessengerProvider messengers)
        {
            _provider = provider;

            this.Users = new NetScopeUserService(this, settings.Get<int>(SettingConstants.MaxRoomUsers).Value);
            this.Incoming = new NetScopeIncomingMessageService(targets);
            this.Outgoing = new NetScopeOutgoingMessageService(this, messengers);
            this.State = NetScopeState.Stopped;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            this.Stop();
        }

        public void Start(byte id)
        {
            if(this.State != NetScopeState.Stopped)
            {
                return;
            }

            this.id = id;
            this.State = NetScopeState.Started;

            // Create the scopes system messenger...
            var entities = _provider.GetRequiredService<IEntityService>();
            entities.TryAdd(this);
            entities.TryCreate<NetSystemMessenger>(out var messenger);
            this.Messenger = messenger;
        }

        public void Stop()
        {
            if (this.State != NetScopeState.Started)
            {
                return;
            }

            this.State = NetScopeState.Stopped;
        }

        public void Clean()
        {
            this.Incoming.Read();
            this.Outgoing.Send(100); // TODO: Update this.
        }
    }
}
