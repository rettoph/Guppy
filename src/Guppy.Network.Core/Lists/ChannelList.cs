using Guppy.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.DependencyInjection;
using Guppy.Lists;
using Guppy.Lists.Interfaces;

namespace Guppy.Network.Lists
{
    public class ChannelList : ServiceList<IChannel>
    {
        #region Private Fields
        private Dictionary<Int16, IChannel> _channels;
        #endregion

        #region Internal Properties
        internal ServiceConfigurationKey channelServiceConfigurationKey { get; set; }
        #endregion

        #region Lifecycle Methods
        protected override void Create(GuppyServiceProvider provider)
        {
            base.Create(provider);

            _channels = new Dictionary<short, IChannel>();

            this.OnAdded += this.HandleChannelAdded;
            this.OnRemoved += this.HandleChannelRemoved;
        }

        protected override void Dispose()
        {
            base.Dispose();

            this.OnAdded -= this.HandleChannelAdded;
            this.OnRemoved -= this.HandleChannelRemoved;
        }
        #endregion

        #region Create Methods
        public override T GetById<T>(Guid id)
        {
            throw new Exception("Unused Method for channel identification. Please use ChannelList.GetById(Int16) instead.");
        }

        public IChannel GetById(Int16 id)
        {
            IChannel channel;
            if (_channels.TryGetValue(id, out channel))
                return channel;

            return this.Create<IChannel>(this.provider, this.channelServiceConfigurationKey, (channel, p, c) =>
            {
                channel.Id = id;
            });
        }
        #endregion

        #region Lifecycle Methods
        private void HandleChannelAdded(IServiceList<IChannel> sender, IChannel args)
        {
            _channels.Add(args.Id, args);
        }

        private void HandleChannelRemoved(IServiceList<IChannel> sender, IChannel args)
        {
            _channels.Remove(args.Id);
        }
        #endregion
    }
}
