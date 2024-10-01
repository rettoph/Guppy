using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Identity.Enums;
using Guppy.Core.Network.Common.Services;
using LiteNetLib;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Core.Network.Identity.Services
{
    internal sealed class NetScopeUserService : INetScopeUserService, IDisposable
    {
        private Dictionary<int, IUser> _users;

        public IEnumerable<NetPeer> Peers
        {
            get
            {
                foreach (User user in _users.Values)
                {
                    if (user.NetPeer is not null)
                    {
                        yield return user.NetPeer;
                    }
                }
            }
        }

        public event OnEventDelegate<INetScopeUserService, IUser>? OnUserJoined;
        public event OnEventDelegate<INetScopeUserService, IUser>? OnUserLeft;

        public NetScopeUserService()
        {
            _users = new Dictionary<int, IUser>();
        }
        public void Dispose()
        {
            while (_users.Any())
            {
                this.Remove(_users.Values.First());
            }
        }

        public void Add(IUser user)
        {
            if (user.State != UserState.Connected)
            {
                return;
            }

            if (_users.TryAdd(user.Id, user))
            {
                user.OnStateChanged += this.HandleUserStateChanged;

                this.OnUserJoined?.Invoke(this, user);
            }
        }

        public void Remove(IUser user)
        {
            if (_users.Remove(user.Id))
            {
                user.OnStateChanged -= this.HandleUserStateChanged;

                this.OnUserLeft?.Invoke(this, user);
            }
        }


        public IUser Get(int id)
        {
            return _users[id];
        }

        public bool TryGet(int id, [MaybeNullWhen(false)] out IUser user)
        {
            return _users.TryGetValue(id, out user);
        }

        private void HandleUserStateChanged(IUser sender, UserState old, UserState value)
        {
            if (value != UserState.Disconnected)
            {
                return;
            }

            this.Remove(sender);
        }

        public IEnumerator<IUser> GetEnumerator()
        {
            return _users.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
