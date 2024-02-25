using Guppy.Network.Identity.Claims;
using Guppy.Network.Identity.Enums;
using LiteNetLib;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Network.Identity.Services
{
    public class UserService : IUserService
    {
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
            _idsUsers = new Dictionary<int, User>();
            _peersUsers = new Dictionary<NetPeer, User>();
        }

        public User GetById(int id)
        {
            return _idsUsers[id];
        }

        public User Create(int id, NetPeer? peer, params Claim[] claims)
        {
            User user = new User(id, peer, claims);
            Add(user);

            return user;
        }

        public User Update(int id, params Claim[] claims)
        {
            User user = GetById(id);
            user.Set(claims);

            return user;
        }

        public User UpdateOrCreate(int id, params Claim[] claims)
        {
            if (TryGet(id, out var user))
            {
                Update(id, claims);
            }
            else
            {
                user = new User(id, null, claims);
                Add(user);
            }

            return user;
        }

        public void Remove(int id)
        {
            if (_idsUsers.Remove(id, out User? user))
            {
                user.State = UserState.Disconnected;
                OnUserDisconnected?.Invoke(this, user);
            }
        }

        public void Add(User user)
        {
            _idsUsers.Add(user.Id, user);
            user.State = UserState.Connected;
            OnUserConnected?.Invoke(this, user);
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
            return GetEnumerator();
        }
    }
}
