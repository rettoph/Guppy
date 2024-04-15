using Guppy.Core.Network.Common.Claims;
using Guppy.Core.Network.Common.Identity.Enums;
using LiteNetLib;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Core.Network.Common
{
    public class User : IUser
    {
        private UserState _state;

        private Dictionary<string, Claim> _claims;

        public int Id { get; }
        public NetPeer? NetPeer { get; }
        public DateTime CreatedAt { get; }
        public UserState State
        {
            get => _state;
            internal set => this.OnStateChanged!.InvokeIf(_state != value, this, ref _state, value);
        }

        public event OnChangedEventDelegate<IUser, UserState> OnStateChanged;

        internal User(int id, IEnumerable<Claim> claims) : this(id, null, claims)
        {

        }
        internal User(int id, NetPeer? netPeer, IEnumerable<Claim> claims)
        {
            this.Id = id;
            this.NetPeer = netPeer;
            this.CreatedAt = DateTime.UtcNow;
            this.State = UserState.Disconnected;
            this.OnStateChanged = null!;

            _claims = claims.ToDictionary(x => x.Key);
        }

        public void Set<T>(string key, T value, ClaimAccessibility accessibility)
        {
            _claims[key] = new Claim<T>(key, value, accessibility);
        }

        public void Set(IEnumerable<Claim> claims)
        {
            foreach (Claim claim in claims)
            {
                _claims[claim.Key] = claim;
            }
        }

        public T Get<T>(string key)
        {
            return ((Claim<T>)_claims[key]).Value;
        }

        public bool TryGet<T>(string key, [MaybeNullWhen(false)] out T? value)
        {
            if (_claims.TryGetValue(key, out var claim) && claim is Claim<T> casted)
            {
                value = casted.Value;
                return true;
            }

            value = default;
            return false;
        }

        public IEnumerator<Claim> GetEnumerator()
        {
            return _claims.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
