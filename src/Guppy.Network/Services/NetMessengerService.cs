using Guppy.Network.Components;
using Guppy.Network.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Services
{
    internal sealed class NetMessengerService : INetMessengerService
    {
        private List<NetMessenger> _messengers;
        private Dictionary<ushort, NetMessenger> _started;

        public NetMessengerService()
        {
            _messengers = new List<NetMessenger>();
            _started = new Dictionary<ushort, NetMessenger>();
        }

        public NetMessenger Get(ushort id)
        {
            return _started[id];
        }

        public bool TryGet(ushort id, [MaybeNullWhen(false)] out NetMessenger messenger)
        {
            return _started.TryGetValue(id, out messenger);
        }

        void INetMessengerService.Add(NetMessenger messenger)
        {
            _messengers.Add(messenger);

            if (messenger.State == NetState.Started)
            {
                this.CleanRoomState(messenger, messenger.State);
            }

            messenger.OnStateChanged += this.HandleRoomStateChanged;
        }

        void INetMessengerService.Remove(NetMessenger messenger)
        {
            _messengers.Remove(messenger);

            this.CleanRoomState(messenger, NetState.Stopped);

            messenger.OnStateChanged -= this.HandleRoomStateChanged;
        }

        private void CleanRoomState(NetMessenger messenger, NetState state)
        {
            if (state == NetState.Started)
            {
                _started.Add(messenger.Id, messenger);
                return;
            }

            if (state == NetState.Stopped)
            {
                _started.Remove(messenger.Id);
                return;
            }
        }

        private void HandleRoomStateChanged(NetMessenger sender, NetState old, NetState value)
        {
            this.CleanRoomState(sender, value);
        }

        public IEnumerator<NetMessenger> GetEnumerator()
        {
            return _messengers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
