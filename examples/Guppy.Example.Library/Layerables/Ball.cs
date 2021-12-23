using Guppy.EntityComponent.DependencyInjection;
using Guppy.Example.Library.Messages;
using Guppy.Network;
using Guppy.Network.Interfaces;
using Guppy.Network.Messages;
using Guppy.Network.Services;
using Guppy.Threading.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Example.Library.Layerables
{
    public class Ball : Positionable, 
        
        IMessageFactory<BallRadiusDto>
    {
        public Single Radius { get; set; }

        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.LayerGroup = Constants.LayerContexts.Foreground.Group.GetValue();

            var room = provider.GetService<RoomService>().GetById(0);

            this.Pipe = room.Pipes.GetById(Guid.Empty);

            this.Position = new Vector2(10, 10);
            this.Radius = 1f;
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.Packets.RegisterPacket<BallRadiusDto, CreateNetworkEntityMessage>(this);
        }

        BallRadiusDto IMessageFactory<BallRadiusDto>.Create()
        {
            return new BallRadiusDto(this.Radius);
        }
    }
}
