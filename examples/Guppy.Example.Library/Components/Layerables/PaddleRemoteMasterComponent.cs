using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Example.Library.Layerables;
using Guppy.Example.Library.Messages;
using Guppy.Network;
using Guppy.Network.Attributes;
using Guppy.Network.Components;
using Guppy.Network.Enums;
using Guppy.Network.Messages;
using Guppy.Threading.Interfaces;
using Guppy.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.Components.Layerables
{
    [HostTypeRequired(HostType.Remote)]
    [NetworkAuthorizationRequired(NetworkAuthorization.Master)]
    public sealed class PaddleRemoteMasterComponent : NetworkComponent<Paddle>,
        IMessageFactory<UserDto>,
        IMessageFactory<PaddleTargetDto>,
        IMessageProcessor<PaddleTargetDto>
    {
        private IntervalInvoker _intervals;

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _intervals);

            _intervals[Library.Constants.Intervals.TargetMessage].OnInterval += this.HandlePaddleTargetMessageInterval;

            this.Entity.Packets.RegisterProcessor<PaddleTargetDto>(this);

            this.Entity.Packets.RegisterPacket<UserDto, CreateNetworkEntityMessage>(this);

            this.Entity.Packets.RegisterPacket<PaddleTargetDto, CreateNetworkEntityMessage>(this);
            this.Entity.Packets.RegisterPacket<PaddleTargetDto, PaddleTargetMessage>(this);
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            this.Entity.Packets.DeregisterProcessor<PaddleTargetDto>();

            this.Entity.Packets.DeregisterPacket<UserDto, CreateNetworkEntityMessage>(this);

            this.Entity.Packets.DeregisterPacket<PaddleTargetDto, CreateNetworkEntityMessage>(this);
            this.Entity.Packets.DeregisterPacket<PaddleTargetDto, PaddleTargetMessage>(this);

            _intervals[Library.Constants.Intervals.TargetMessage].OnInterval -= this.HandlePaddleTargetMessageInterval;

            _intervals = default;
        }
        #endregion

        #region IMessageFactory Implementations
        UserDto IMessageFactory<UserDto>.Create()
        {
            return new UserDto(this.Entity.User.Id);
        }

        PaddleTargetDto IMessageFactory<PaddleTargetDto>.Create()
        {
            return new PaddleTargetDto(this.Entity.Target);
        }
        #endregion

        #region IMessageProcessor Implementations
        public void Process(PaddleTargetDto message)
        {
            this.Entity.Target = message.Target;
        }
        #endregion

        private void HandlePaddleTargetMessageInterval(GameTime gameTime)
        {
            this.Entity.SendMessage<PaddleTargetMessage>();
        }
    }
}
