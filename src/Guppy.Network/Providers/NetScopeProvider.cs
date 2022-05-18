using Guppy.EntityComponent.Services;
using Guppy.Network.Enums;
using Guppy.Providers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Providers
{
    internal sealed class NetScopeProvider : INetScopeProvider
    {
        private HashSet<NetScope> _scopes;
        private Dictionary<byte, NetScope> _started;

        public NetScopeProvider()
        {
            _scopes = new HashSet<NetScope>();
            _started = new Dictionary<byte, NetScope>();
        }

        public bool TryGet(byte id, [MaybeNullWhen(false)] out NetScope room)
        {
            return _started.TryGetValue(id, out room);
        }

        bool INetScopeProvider.TryAdd(NetScope scope)
        {
            if(_scopes.Add(scope))
            {
                this.CleanRoomState(scope, scope.State);
                scope.OnStateChanged += this.HandleRoomStateChanged;

                return true;
            }

            return false;
        }

        bool INetScopeProvider.TryRemove(NetScope scope)
        {
            if(_scopes.Remove(scope))
            {
                this.CleanRoomState(scope, NetScopeState.Stopped);
                scope.OnStateChanged -= this.HandleRoomStateChanged;

                return true;
            }

            return false;
        }

        private void CleanRoomState(NetScope scope, NetScopeState state)
        {
            if (state == NetScopeState.Started)
            {
                _started.Add(scope.Id, scope);
                return;
            }

            if (state == NetScopeState.Stopped)
            {
                _started.Remove(scope.Id);
                return;
            }
        }

        private void HandleRoomStateChanged(NetScope sender, NetScopeState old, NetScopeState value)
        {
            this.CleanRoomState(sender, value);
        }

        public IEnumerator<NetScope> GetEnumerator()
        {
            return _scopes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
