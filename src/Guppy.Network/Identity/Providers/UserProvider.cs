using Guppy.Network.Identity.Claims;
using Guppy.Network.Identity.Enums;
using LiteNetLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Identity.Providers
{
    public class UserProvider : IUserProvider
    {
        private Dictionary<int, User> _users;

        public IEnumerable<NetPeer> Peers
        {
            get {
                foreach (User user in _users.Values)
                {
                    if (user.NetPeer is not null)
                    {
                        yield return user.NetPeer;
                    }
                }
            }
        }

        User? IUserProvider.Current { get; set; }

        public event OnEventDelegate<IUserProvider, User>? OnUserConnected;
        public event OnEventDelegate<IUserProvider, User>? OnUserDisconnected;

        public UserProvider()
        {
            _users = new Dictionary<int, User>();
        }

        public User Get(int id)
        {
            return _users[id];
        }

        private User Update(int id, NetPeer? peer, bool updatePeer, params Claim[] claims)
        {
            var user = this.Get(id);
            user.Set(claims);

            if(updatePeer)
            {
                user.NetPeer = peer;
            }

            return user;
        }

        public User Update(int id, NetPeer? peer, params Claim[] claims)
        {
            return this.Update(id, peer, true, claims);
        }

        public User Update(int id, params Claim[] claims)
        {
            return this.Update(id, null, false, claims);
        }

        public User UpdateOrCreate(int id, NetPeer? peer, bool updatePeer, params Claim[] claims)
        {
            if (this.TryGet(id, out var user))
            {
                this.Update(id, peer, updatePeer, claims);
            }
            else
            {
                user = new User(id, peer, claims);
                this.Add(user);
            }

            return user;
        }

        public User UpdateOrCreate(int id, NetPeer? peer, params Claim[] claims)
        {
            return this.UpdateOrCreate(id, peer, true, claims);
        }

        public User UpdateOrCreate(int id, params Claim[] claims)
        {
            return this.UpdateOrCreate(id, null, false, claims);
        }

        public void Remove(int id)
        {
            if(_users.Remove(id, out var user))
            {
                user.State = UserState.Disconnected;
                this.OnUserDisconnected?.Invoke(this, user);
            }
        }

        public void Add(User user)
        {
            _users.Add(user.Id, user);
            user.State = UserState.Connected;
            this.OnUserConnected?.Invoke(this, user);
        }

        public bool TryGet(int id, [MaybeNullWhen(false)] out User user)
        {
            return _users.TryGetValue(id, out user);
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
