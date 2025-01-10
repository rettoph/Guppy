using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Identity.Enums;
using Guppy.Core.Network.Common.Services;
using LiteNetLib;

namespace Guppy.Core.Network.Identity.Services
{
    internal sealed class NetScopeUserService : INetScopeUserService, IDisposable
    {
        private readonly Dictionary<int, IUser> _users;

        public IEnumerable<NetPeer> Peers
        {
            get
            {
                foreach (User user in this._users.Values.Cast<User>())
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
            this._users = [];
        }
        public void Dispose()
        {
            while (this._users.Count != 0)
            {
                this.Remove(this._users.Values.First());
            }
        }

        public void Add(IUser user)
        {
            if (user.State != UserStateEnum.Connected)
            {
                return;
            }

            if (this._users.TryAdd(user.Id, user))
            {
                user.OnStateChanged += this.HandleUserStateChanged;

                this.OnUserJoined?.Invoke(this, user);
            }
        }

        public void Remove(IUser user)
        {
            if (this._users.Remove(user.Id))
            {
                user.OnStateChanged -= this.HandleUserStateChanged;

                this.OnUserLeft?.Invoke(this, user);
            }
        }


        public IUser Get(int id) => this._users[id];

        public bool TryGet(int id, [MaybeNullWhen(false)] out IUser user) => this._users.TryGetValue(id, out user);

        private void HandleUserStateChanged(IUser sender, UserStateEnum old, UserStateEnum value)
        {
            if (value != UserStateEnum.Disconnected)
            {
                return;
            }

            this.Remove(sender);
        }

        public IEnumerator<IUser> GetEnumerator() => this._users.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}