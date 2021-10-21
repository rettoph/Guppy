using Guppy.DependencyInjection;
using Guppy.Events.Delegates;
using Guppy.Extensions.log4net;
using Guppy.Extensions.System.Collections;
using Guppy.Network.Channels;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Interfaces;
using Guppy.Network.Lists;
using Guppy.Network.Security;
using Guppy.Network.Services;
using Guppy.Utilities;
using Lidgren.Network;
using log4net;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using Guppy.Utilities.Threading;
using Microsoft.Xna.Framework;
using Guppy.Network.Structs;

namespace Guppy.Network.Peers
{
    public abstract class Peer : Service, IPeer
    {
        #region Private Fields
        private GuppyServiceProvider _provider;
        private ILog _log;
        private NetOutgoingMessageService _outgoing;
        private NetPeer _peer;
        private NetIncomingMessage _im;

        private CancellationTokenSource _tokenSource;
        private Task _loop;

        private Action<GameTime> _update;
        private Double _diagnosticInterval;
        private ActionTimer _diagnosticTimer;
        private UInt32 _flushed;
        private UInt32 _sent;
        private UInt32 _recieved;
        #endregion

        #region Public Properties
        /// <summary>
        /// By default all <see cref="Peer"/> id's are set to <see cref="Int16.MinValue"/>.
        /// </summary>
        public new Int16 Id => Int16.MinValue;

        public Double DiagnosticInterval
        {
            get => _diagnosticInterval;
            set
            {
                _diagnosticInterval = value;

                if (_diagnosticInterval > 0)
                {
                    _update = this.UpdateDiagnostics;
                }
                else
                {
                    _update = this.Update;
                }
            }
        }
        #endregion

        #region Protected Properties
        /// <summary>
        /// The <see cref="ServiceConfigurationDescriptor.Key"/> to be used when creating a new
        /// <see cref="IChannel"/> instance.
        /// </summary>
        protected abstract ServiceConfigurationKey channelServiceConfigurationkey { get; }
        #endregion

        #region Events
        public event OnEventDelegate<IPeer, DiagnosticIntervalData> OnDiagnosticInterval;
        #endregion

        #region IPeer Implementation
        /// <inheritdoc />
        public UserList Users { get; private set; }

        /// <inheritdoc />
        public ChannelList Channels { get; private set; }

        /// <inheritdoc />
        public IUser CurrentUser { get; internal set; }

        /// <inheritdoc />
        public Dictionary<NetIncomingMessageType, OnEventDelegate<IPeer, NetIncomingMessage>> OnIncomingMessageTypeRecieved { get; private set; }

        /// <inheritdoc />
        public event OnEventDelegate<IPeer, NetIncomingMessage> OnIncomingMessageRecieved;

        /// <inheritdoc />
        ServiceConfigurationKey IPeer.ChannelServiceConfigurationKey => this.channelServiceConfigurationkey;
        #endregion

        #region Lifecycle Methods
        protected override void Create(GuppyServiceProvider provider)
        {
            base.Create(provider);

            this.OnIncomingMessageTypeRecieved = DictionaryHelper.BuildEnumDictionary<NetIncomingMessageType, OnEventDelegate<IPeer, NetIncomingMessage>>();

            this.OnIncomingMessageRecieved += this.HandleIncomingMessageRecieved;
        }

        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.DiagnosticInterval = 0;
            this.Users = provider.GetService<UserList>();
            this.Channels = provider.GetService<ChannelList>((channels, p, c) =>
            {
                channels.channelServiceConfigurationKey = this.channelServiceConfigurationkey;
            });
        }

        protected override void Initialize(GuppyServiceProvider provider)
        {
            base.Initialize(provider);

            _provider = provider;

            provider.Service(out _peer);
            provider.Service(out _log);
            provider.Service(out _outgoing);

            _diagnosticTimer = new ActionTimer(1000);

            this.OnIncomingMessageTypeRecieved[NetIncomingMessageType.Data] += this.HandleDataMessage;
            this.OnIncomingMessageTypeRecieved[NetIncomingMessageType.DebugMessage] += this.HandleDebugMessage;
            this.OnIncomingMessageTypeRecieved[NetIncomingMessageType.VerboseDebugMessage] += this.HandleVerboseDebugMessage;
            this.OnIncomingMessageTypeRecieved[NetIncomingMessageType.ErrorMessage] += this.HandleErrorMessage;
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            this.Users.TryRelease();
            this.Channels.TryRelease();

            _peer = null;
            _log = null;
            _outgoing = null;
            this.Users = null;
            this.Channels = null;
        }

        protected override void Dispose()
        {
            base.Dispose();

            this.OnIncomingMessageTypeRecieved.Clear();

            this.OnIncomingMessageTypeRecieved = null;

            this.OnIncomingMessageTypeRecieved[NetIncomingMessageType.Data] -= this.HandleDataMessage;
            this.OnIncomingMessageTypeRecieved[NetIncomingMessageType.DebugMessage] -= this.HandleDebugMessage;
            this.OnIncomingMessageTypeRecieved[NetIncomingMessageType.VerboseDebugMessage] -= this.HandleVerboseDebugMessage;
            this.OnIncomingMessageTypeRecieved[NetIncomingMessageType.ErrorMessage] -= this.HandleErrorMessage;
        }
        #endregion

        #region Helper Methods
        /// <inheritdoc />
        public async Task StartAsync(Int32? updateIntervalMilliseconds = null)
        {
            _peer.Start();

            if(updateIntervalMilliseconds is not null)
            {
                _tokenSource = new CancellationTokenSource();

                _loop = TaskHelper.CreateLoop(gt =>
                {
                    this.TryUpdate(gt);
                }, updateIntervalMilliseconds.Value, _tokenSource.Token);

                await _loop;
            }
        }

        /// <inheritdoc />
        public virtual async Task StopAsync()
        {
            _tokenSource.Cancel();

            await _loop;
        }

        /// <inheritdoc />
        public IUser CreateUser(params Claim[] claims)
            => _provider.GetService<IUser>((user, p, c) =>
            {
                claims.ForEach(claim => user.AddClaim(claim));
            });
        #endregion

        #region Frame Methods
        public virtual void TryUpdate(GameTime gameTime)
        {
            _update(gameTime);
        }

        protected virtual void Update(GameTime gameTime)
        {
            while ((_im = _peer.ReadMessage()) != default)
                this.OnIncomingMessageRecieved.Invoke(this, _im);

            _outgoing.Flush(out _, out _);
        }

        protected virtual void UpdateDiagnostics(GameTime gameTime)
        {
            UInt32 flushed = 0, sent = 0, recieved= 0;

            while ((_im = _peer.ReadMessage()) != default)
            {
                this.OnIncomingMessageRecieved.Invoke(this, _im);
                recieved++;
            }
                
            _outgoing.Flush(out flushed, out sent);

            _flushed += flushed;
            _sent += sent;
            _recieved += recieved;

            _diagnosticTimer.Update(gameTime, this.TrackDiagnosticsSecond);
        }

        protected virtual void TrackDiagnosticsSecond(GameTime gameTime)
        {
            this.OnDiagnosticInterval?.Invoke(this, new DiagnosticIntervalData(_flushed, _sent, _recieved));

            _flushed = 0;
            _sent = 0;
            _recieved = 0;
        }
        #endregion

        #region MessageRecieved Handlers
        private void HandleIncomingMessageRecieved(IPeer sender, NetIncomingMessage im)
        {
            this.OnIncomingMessageTypeRecieved[_im.MessageType]?.Invoke(this, _im);
        }

        private void HandleDataMessage(IPeer sender, NetIncomingMessage im)
        {
            // There is a global agreement that all custom messages are handled by a recieving
            // channel, as signed by their prefix value.
            var channelId = im.ReadInt16();
            this.Channels.GetById(channelId).Messages.Read(im);
        }

        private void HandleDebugMessage(IPeer sender, NetIncomingMessage im)
        {
            _log.Debug($"{im.ReadString()}");
        }

        private void HandleVerboseDebugMessage(IPeer sender, NetIncomingMessage im)
        {
            _log.Verbose($"{im.ReadString()}");
        }

        private void HandleErrorMessage(IPeer sender, NetIncomingMessage im)
        {
            _log.Error($"{im.ReadString()}");
        }
        #endregion
    }
}
