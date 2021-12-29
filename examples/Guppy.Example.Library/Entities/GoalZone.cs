using Guppy.EntityComponent.DependencyInjection;
using Guppy.Example.Library.Messages;
using Guppy.Network;
using Guppy.Network.Messages;
using Guppy.Network.Services;
using Guppy.Threading.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.Entities
{
    public class GoalZone : NetworkLayerable,
        IMessageFactory<GoalZoneDto>, IMessageProcessor<GoalZoneDto>
    {
        List<Paddle> _paddles;

        public Paddle Owner { get; set; }
        public RectangleF Bounds { get; set; }

        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            var room = provider.GetService<RoomService>().GetById(0);
            this.Pipe = room.Pipes.GetById(Guid.Empty);

            provider.Service(out _paddles);

            this.Messages.RegisterPacket<GoalZoneDto, CreateNetworkEntityMessage>(this);
            this.Messages.RegisterProcessor<GoalZoneDto>(this);
        }

        protected override void Release()
        {
            base.Release();

            this.Messages.DeregisterPacket<GoalZoneDto, CreateNetworkEntityMessage>(this);
            this.Messages.DeregisterProcessor<GoalZoneDto>(this);
        }

        public GoalZoneDto Create()
        {
            return new GoalZoneDto(this.Owner.NetworkId, this.Bounds.X, this.Bounds.Y, this.Bounds.Width, this.Bounds.Height);
        }

        public void Process(GoalZoneDto message)
        {
            this.Owner = _paddles.First(paddle => paddle.NetworkId == message.OwnerNetworkId);
            this.Bounds = new RectangleF(message.X, message.Y, message.Width, message.Height);
        }
    }
}
