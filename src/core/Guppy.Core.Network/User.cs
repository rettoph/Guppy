using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Claims;
using Guppy.Core.Network.Common.Identity.Enums;
using LiteNetLib;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Core.Network
{
    public class User : IUser, IDisposable
    {
        private UserState _state;

        private readonly Dictionary<string, Claim> _claims;

        public int Id { get; private set; }
        public NetPeer? NetPeer { get; private set; }
        public UserState State
        {
            get => _state;
            private set => this.OnStateChanged!.InvokeIf(_state != value, this, ref _state, value);
        }

        public event OnChangedEventDelegate<IUser, UserState> OnStateChanged;

        internal User(IEnumerable<Claim> claims)
        {
            this.State = UserState.Disconnected;
            this.OnStateChanged = null!;

            _claims = claims.ToDictionary(x => x.Key);
        }

        public User Initialize(int id, NetPeer? netPeer)
        {
            this.Id = id;
            this.NetPeer = netPeer;
            this.State = UserState.Connected;

            return this;
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

        public void Dispose()
        {
            this.State = UserState.Disconnected;
        }
    }
}
