using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Claims;
using Guppy.Core.Network.Common.Identity.Enums;
using LiteNetLib;

namespace Guppy.Core.Network
{
    public class User : IUser, IDisposable
    {
        private UserStateEnum _state;
        private bool _disposed;
        private readonly Dictionary<string, Claim> _claims;

        public int Id { get; private set; }
        public NetPeer? NetPeer { get; private set; }
        public UserStateEnum State
        {
            get => this._state;
            private set => this.OnStateChanged!.InvokeIf(this._state != value, this, ref this._state, value);
        }

        public event OnChangedEventDelegate<IUser, UserStateEnum> OnStateChanged;

        internal User(IEnumerable<Claim> claims)
        {
            this.State = UserStateEnum.Disconnected;
            this.OnStateChanged = null!;

            this._claims = claims.ToDictionary(x => x.Key);
        }

        public User Initialize(int id, NetPeer? netPeer)
        {
            this.Id = id;
            this.NetPeer = netPeer;
            this.State = UserStateEnum.Connected;

            return this;
        }

        public void Set<T>(string key, T value, ClaimAccessibilityEnum accessibility) => this._claims[key] = new Claim<T>(key, value, accessibility);

        public void Set(IEnumerable<Claim> claims)
        {
            foreach (Claim claim in claims)
            {
                this._claims[claim.Key] = claim;
            }
        }

        public T Get<T>(string key) => ((Claim<T>)this._claims[key]).Value;

        public bool TryGet<T>(string key, [MaybeNullWhen(false)] out T? value)
        {
            if (this._claims.TryGetValue(key, out var claim) && claim is Claim<T> casted)
            {
                value = casted.Value;
                return true;
            }

            value = default;
            return false;
        }

        public IEnumerator<Claim> GetEnumerator() => this._claims.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    this.State = UserStateEnum.Disconnected;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                this._disposed = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~User()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}