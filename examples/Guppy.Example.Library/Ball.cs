using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Network;
using Guppy.Network.Contexts;
using Guppy.Network.Interfaces;
using Guppy.Network.Scenes;
using Guppy.Network.Utilities;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Extensions.Lidgren;

namespace Guppy.Example.Library
{
    public class Ball : NetworkLayerable
    {
        private IChannel _channel;
        public Vector2 Position;

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _channel = provider.GetService<NetworkScene>().Channel;
            this.Pipe = _channel.Pipes.GetOrCreateById(Guid.Empty);

            this.Messages[Guppy.Network.Constants.Messages.NetworkEntity.Create].OnWrite += this.WriteCreateMessage;
            this.Messages[Guppy.Network.Constants.Messages.NetworkEntity.Create].OnRead += this.ReadCreateMessage;
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private void ReadCreateMessage(MessageTypeManager sender, NetIncomingMessage args)
        {
            this.Position = args.ReadVector2();
        }

        private void WriteCreateMessage(MessageTypeManager sender, NetOutgoingMessage args)
        {
            args.Write(this.Position);
        }
    }
}
