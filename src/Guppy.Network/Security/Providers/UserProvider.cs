using Guppy.Network.Security.Providers;
using Guppy.Network.Security.Structs;
using LiteNetLib;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Security.Providers
{
    internal sealed class UserProvider : IUserProvider
    {
        private ConcurrentDictionary<int, User> _users;
        private NetManager _manager;

        public UserProvider(NetManager manager)
        {
            _users = new ConcurrentDictionary<int, User>();
            _manager = manager;
        }

        public bool TryGet(int id, [MaybeNullWhen(false)] out User user)
        {
            return _users.TryGetValue(id, out user);
        }

        public User UpdateOrCreate(int id, IEnumerable<Claim> claims)
        {
            lock (this)
            {
                if (this.TryGet(id, out User? user))
                {
                    user.SetClaims(claims);
                    return user;
                }

                user = new User(id, claims, null);

                if (_users.TryAdd(id, user))
                {
                    return user;
                }

                throw new InvalidOperationException();
            }
        }
        public User UpdateOrCreate(int id, IEnumerable<Claim> claims, NetPeer? peer)
        {
            lock(this)
            {
                if(this.TryGet(id, out User? user))
                {
                    user.SetClaims(claims);
                    user.NetPeer = peer;
                    return user;
                }

                user = new User(id, claims, peer);

                if(_users.TryAdd(id, user))
                {
                    return user;
                }

                throw new InvalidOperationException();
            }
        }

        public IEnumerator<User> GetEnumerator()
        {
            return _users.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _users.Values.GetEnumerator();
        }
    }
}
