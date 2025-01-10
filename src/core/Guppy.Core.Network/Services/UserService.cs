using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Claims;
using Guppy.Core.Network.Common.Dtos;
using Guppy.Core.Network.Common.Identity.Enums;
using Guppy.Core.Network.Common.Services;
using LiteNetLib;

namespace Guppy.Core.Network.Services
{
    public class UserService : IUserService
    {
        private readonly Dictionary<int, User> _idsUsers;
        private readonly Dictionary<NetPeer, User> _peersUsers;

        public IEnumerable<NetPeer> Peers
        {
            get
            {
                foreach (User user in this._idsUsers.Values)
                {
                    if (user.NetPeer is not null)
                    {
                        yield return user.NetPeer;
                    }
                }
            }
        }

        public User Current { get; }
        IUser IUserService.Current => this.Current;

        public event OnEventDelegate<IUserService, IUser>? OnUserConnected;
        public event OnEventDelegate<IUserService, IUser>? OnUserDisconnected;

        public UserService()
        {
            this._idsUsers = [];
            this._peersUsers = [];

            this.Current = this.Create([]);
        }

        public User GetById(int id) => this._idsUsers[id];

        IUser IUserService.GetById(int id) => this.GetById(id);

        public bool TryGet(int id, [MaybeNullWhen(false)] out User user) => this._idsUsers.TryGetValue(id, out user);

        bool IUserService.TryGet(int id, [MaybeNullWhen(false)] out IUser user)
        {
            if (this.TryGet(id, out User? source))
            {
                user = source;
                return true;
            }

            user = null!;
            return false;
        }

        public IUser GetByNetPeer(NetPeer peer) => this._peersUsers[peer];

        public User Create(Claim[] claims, params Claim[] additionalClaims)
        {
            User user = new(claims.Concat(additionalClaims));
            user.OnStateChanged += this.HandleUserStateChanged;

            return user;
        }

        public User Update(int id, IEnumerable<Claim> claims)
        {
            User user = this.GetById(id);
            user.Set(claims);

            return user;
        }

        public IUser UpdateOrCreate(UserDto userDto)
        {
            if (this.TryGet(userDto.Id, out var user))
            {
                this.Update(userDto.Id, userDto.Claims);
            }
            else
            {
                user = new User(userDto.Claims);
            }

            return user;
        }

        public void Remove(int id)
        {
            if (this.TryGet(id, out User? user))
            {
                user.Dispose();
            }
        }

        IEnumerator<IUser> IEnumerable<IUser>.GetEnumerator() => throw new NotImplementedException();

        public IEnumerator<User> GetEnumerator() => this._idsUsers.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        private void HandleUserStateChanged(IUser sender, UserStateEnum old, UserStateEnum value)
        {
            if (sender is not User user)
            {
                throw new ArgumentException($"Sender is {sender.GetType().Name}, {nameof(User)} expected.");
            }

            if (value == UserStateEnum.Connected)
            {
                this._idsUsers.Add(user.Id, user);
                if (user.NetPeer is not null)
                {
                    this._peersUsers.Add(user.NetPeer, user);
                }

                this.OnUserConnected?.Invoke(this, user);

                return;
            }

            if (value == UserStateEnum.Disconnected)
            {
                user.OnStateChanged -= this.HandleUserStateChanged;

                if (this._idsUsers.Remove(user.Id) == false)
                {
                    return;
                }

                if (sender.NetPeer is not null)
                {
                    this._peersUsers.Remove(sender.NetPeer);
                }

                this.OnUserDisconnected?.Invoke(this, user);

                return;
            }

            throw new UnreachableException();
        }
    }
}