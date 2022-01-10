using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Threading.Interfaces;
using Guppy.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Utilities
{
    /// <summary>
    /// Represents a single message that will always be broadcasted at
    /// an interval, when applicable.
    /// </summary>
    public sealed class NetworkEntityMessagePinger : Service
    {
        #region Private Fields
        private Boolean _dirty;
        private Boolean _enabled;
        private Boolean _pingEnqueued;
        private Double _lastPing;

        private IntervalQueue _minimumIntervalQueue;
        private Interval _maximumInterval;
        #endregion

        #region Internal Methods
        internal Action sender;
        #endregion

        #region Public Properties
        /// <summary>
        /// The minimum amount of time that may pass between
        /// a broadcast.
        /// </summary>
        public Double MinimumInterval { get; internal set; }

        /// <summary>
        /// The maximum amount of time that may pass between a broadcast.
        /// </summary>
        public Double MaximumInterval { get; internal set; }

        /// <summary>
        /// When true, the message will be broadcasted next <see cref="MinimumInterval"/>.
        /// </summary>
        public Boolean Dirty
        {
            get => _dirty;
            set => this.OnDirtyChanged.InvokeIf(_dirty != value, this, ref _dirty, value);
        }

        /// <summary>
        /// Indicates whether or not the entire pinger is currently enabled.
        /// </summary>
        public Boolean Enabled
        {
            get => _enabled;
            set => this.OnEnabledChanged.InvokeIf(_enabled != value, this, ref _enabled, value);
        }
        #endregion

        #region Events
        /// <summary>
        /// Invoked when <see cref="Dirty"/> is changed.
        /// </summary>
        public event OnEventDelegate<NetworkEntityMessagePinger, Boolean> OnDirtyChanged;

        /// <summary>
        /// Invoked when <see cref="Enabled"/> is changed.
        /// </summary>
        public event OnEventDelegate<NetworkEntityMessagePinger, Boolean> OnEnabledChanged;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _minimumIntervalQueue = provider.GetIntervalQueue(this.MinimumInterval);
            _maximumInterval = provider.GetInterval(this.MaximumInterval);

            this.OnEnabledChanged += this.HandleEnabledChanged;
            this.CleanEnabled(this.Enabled);
        }

        protected override void Uninitialize()
        {
            base.Uninitialize();

            this.OnEnabledChanged -= this.HandleEnabledChanged;
            this.CleanEnabled(false);
        }
        #endregion

        #region Helper Methods
        private void TryEnqueuePing()
        {
            if(_pingEnqueued)
            {
                return;
            }

            _minimumIntervalQueue.Enqueue(this.Id, this.Ping);
            _pingEnqueued = true;
        }

        private void Ping(Guid id, GameTime gameTime)
        {
            sender();

            this.Dirty = false;
            _lastPing = gameTime.TotalGameTime.TotalMilliseconds;
            _pingEnqueued = false;
        }

        private void CleanEnabled(Boolean enabled)
        {
            if(enabled)
            {
                _maximumInterval.OnInterval += this.HandleMaximumInterval;
                this.OnDirtyChanged += this.HandleDirtyChanged;

                if (this.Dirty)
                {
                    this.TryEnqueuePing();
                }
            }
            else
            {
                _maximumInterval.OnInterval -= this.HandleMaximumInterval;
                this.OnDirtyChanged -= this.HandleDirtyChanged;
            }
        }
        #endregion

        #region Event Handlers
        private void HandleEnabledChanged(NetworkEntityMessagePinger sender, bool enabled)
        {
            this.CleanEnabled(enabled);
        }

        private void HandleMaximumInterval(GameTime gameTime)
        {
            if(gameTime.TotalGameTime.TotalMilliseconds - _lastPing > this.MaximumInterval)
            {
                this.Dirty = true;
            }
        }

        private void HandleDirtyChanged(NetworkEntityMessagePinger sender, bool dirty)
        {
            if(dirty)
            {
                this.TryEnqueuePing();
            }
        }
        #endregion
    }
}
