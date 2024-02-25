using Guppy.Network.Identity.Enums;
using LiteNetLib;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Network.Identity.Services
{
    internal sealed class NetScopeUserService : INetScopeUserService, IDisposable
    {
        private Dictionary<int, User> _users;

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

        public event OnEventDelegate<INetScopeUserService, User>? OnUserJoined;
        public event OnEventDelegate<INetScopeUserService, User>? OnUserLeft;

        public NetScopeUserService()
        {
            _users = new Dictionary<int, User>();
        }
        public void Dispose()
        {
            while (_users.Any())
            {
                this.Remove(_users.Values.First());
            }
        }

        public void Add(User user)
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

        public void Remove(User user)
        {
            if (_users.Remove(user.Id))
            {
                user.OnStateChanged -= this.HandleUserStateChanged;

                this.OnUserLeft?.Invoke(this, user);
            }
        }


        public User Get(int id)
        {
            return _users[id];
        }

        public bool TryGet(int id, [MaybeNullWhen(false)] out User user)
        {
            return _users.TryGetValue(id, out user);
        }

        private void HandleUserStateChanged(User sender, UserState old, UserState value)
        {
            if (value != UserState.Disconnected)
            {
                return;
            }

            this.Remove(sender);
        }

        public IEnumerator<User> GetEnumerator()
        {
            return _users.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
