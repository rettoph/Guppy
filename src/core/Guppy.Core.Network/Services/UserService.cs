using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Claims;
using Guppy.Core.Network.Common.Dtos;
using Guppy.Core.Network.Common.Identity.Enums;
using Guppy.Core.Network.Common.Services;
using LiteNetLib;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Core.Network.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly Dictionary<int, User> _idsUsers;
        private readonly Dictionary<NetPeer, User> _peersUsers;

        public IEnumerable<NetPeer> Peers
        {
            get
            {
                foreach (User user in _idsUsers.Values)
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
            _idsUsers = new Dictionary<int, User>();
            _peersUsers = new Dictionary<NetPeer, User>();

            this.Current = this.Create(Array.Empty<Claim>());
        }

        public User GetById(int id)
        {
            return _idsUsers[id];
        }

        IUser IUserService.GetById(int id)
        {
            return this.GetById(id);
        }

        public bool TryGet(int id, [MaybeNullWhen(false)] out User user)
        {
            return _idsUsers.TryGetValue(id, out user);
        }

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

        public IUser GetByNetPeer(NetPeer peer)
        {
            return _peersUsers[peer];
        }

        public User Create(Claim[] claims, params Claim[] additionalClaims)
        {
            User user = new User(claims.Concat(additionalClaims));
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

        IEnumerator<IUser> IEnumerable<IUser>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<User> GetEnumerator()
        {
            return _idsUsers.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void HandleUserStateChanged(IUser sender, UserState old, UserState value)
        {
            if (sender is not User user)
            {
                throw new ArgumentException();
            }

            if (value == UserState.Connected)
            {
                _idsUsers.Add(user.Id, user);
                if (user.NetPeer is not null)
                {
                    _peersUsers.Add(user.NetPeer, user);
                }

                this.OnUserConnected?.Invoke(this, user);

                return;
            }

            if (value == UserState.Disconnected)
            {
                user.OnStateChanged -= this.HandleUserStateChanged;

                if (_idsUsers.Remove(user.Id) == false)
                {
                    return;
                }

                if (sender.NetPeer is not null)
                {
                    _peersUsers.Remove(sender.NetPeer);
                }

                this.OnUserDisconnected?.Invoke(this, user);

                return;
            }

            throw new UnreachableException();
        }
    }
}
