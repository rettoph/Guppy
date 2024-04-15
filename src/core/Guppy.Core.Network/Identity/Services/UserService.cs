using Guppy.Core.Network.Identity.Claims;
using Guppy.Core.Network.Identity.Dtos;
using Guppy.Core.Network.Identity.Enums;
using LiteNetLib;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Core.Network.Identity.Services
{
    public class UserService : IUserService
    {
        private int _nextUserId;
        private Dictionary<int, User> _idsUsers;
        private Dictionary<NetPeer, User> _peersUsers;

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

        User? IUserService.Current { get; set; }

        public event OnEventDelegate<IUserService, User>? OnUserConnected;
        public event OnEventDelegate<IUserService, User>? OnUserDisconnected;

        public UserService()
        {
            _nextUserId = 0;
            _idsUsers = new Dictionary<int, User>();
            _peersUsers = new Dictionary<NetPeer, User>();
        }

        public User GetById(int id)
        {
            return _idsUsers[id];
        }

        public User GetByNetPeer(NetPeer peer)
        {
            return _peersUsers[peer];
        }

        public User Create(NetPeer? peer, Claim[] claims, params Claim[] additionalClaims)
        {
            User user = new User(_nextUserId++, peer, claims.Concat(additionalClaims));
            this.Add(user);

            return user;
        }

        public User Create(NetPeer? peer, UserDto userDto, params Claim[] additionalClaims)
        {
            User user = new User(userDto.Id, peer, userDto.Claims.Concat(additionalClaims));
            this.Add(user);
            _nextUserId = Math.Max(userDto.Id + 1, _nextUserId);

            return user;
        }

        public User Update(int id, IEnumerable<Claim> claims)
        {
            User user = this.GetById(id);
            user.Set(claims);

            return user;
        }

        public User UpdateOrCreate(UserDto userDto)
        {
            if (this.TryGet(userDto.Id, out var user))
            {
                this.Update(userDto.Id, userDto.Claims);
            }
            else
            {
                user = new User(userDto.Id, null, userDto.Claims);
                this.Add(user);
            }

            return user;
        }

        public void Remove(int id)
        {
            if (_idsUsers.Remove(id, out User? user))
            {
                if (user.NetPeer is not null)
                {
                    _peersUsers.Remove(user.NetPeer);
                }

                user.State = UserState.Disconnected;
                this.OnUserDisconnected?.Invoke(this, user);
            }
        }

        public void Add(User user)
        {
            _idsUsers.Add(user.Id, user);
            if (user.NetPeer is not null)
            {
                _peersUsers.Add(user.NetPeer, user);
            }

            user.State = UserState.Connected;
            this.OnUserConnected?.Invoke(this, user);
        }

        public bool TryGet(int id, [MaybeNullWhen(false)] out User user)
        {
            return _idsUsers.TryGetValue(id, out user);
        }

        public IEnumerator<User> GetEnumerator()
        {
            return _idsUsers.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
