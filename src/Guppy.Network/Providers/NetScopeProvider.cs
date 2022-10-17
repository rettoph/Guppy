using Guppy.Network.Enums;
using Guppy.Network.Identity.Providers;
using Guppy.Resources;
using Guppy.Resources.Providers;
using Microsoft.Extensions.DependencyInjection;
using System;
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

        public NetScopeProvider(ISettingProvider settings)
        {
            _scopes = new HashSet<NetScope>();
            _started = new Dictionary<byte, NetScope>();
        }

        public NetScope Get(byte id)
        {
            return _started[id];
        }

        public bool TryGet(byte id, [MaybeNullWhen(false)] out NetScope scope)
        {
            return _started.TryGetValue(id, out scope);
        }

        public NetScope Create(IServiceProvider provider)
        {
            var scope = ActivatorUtilities.CreateInstance<NetScope>(provider);

            this.Add(scope);

            return scope;
        }

        private void Add(NetScope scope)
        {
            if (_scopes.Add(scope))
            {
                if (scope.State == NetState.Started)
                {
                    this.CleanRoomState(scope, scope.State);
                }

                scope.OnStateChanged += this.HandleRoomStateChanged;
            }
        }

        private void Remove(NetScope scope)
        {
            if (_scopes.Remove(scope))
            {
                this.CleanRoomState(scope, NetState.Stopped);
                scope.OnStateChanged -= this.HandleRoomStateChanged;
            }
        }

        private void CleanRoomState(NetScope scope, NetState state)
        {
            if (state == NetState.Started)
            {
                _started.Add(scope.Id, scope);
                return;
            }

            if (state == NetState.Stopped)
            {
                _started.Remove(scope.Id);
                return;
            }

            if(state == NetState.Disposed)
            {
                this.Remove(scope);
                return;
            }
        }

        private void HandleRoomStateChanged(NetScope sender, NetState old, NetState value)
        {
            this.CleanRoomState(sender, value);
        }
    }
}
