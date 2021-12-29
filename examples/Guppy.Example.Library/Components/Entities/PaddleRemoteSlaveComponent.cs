using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Example.Library.Entities;
using Guppy.Example.Library.Messages;
using Guppy.Network.Attributes;
using Guppy.Network.Components;
using Guppy.Network.Enums;
using Guppy.Network.Messages;
using Guppy.Network.Security;
using Guppy.Threading.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.Components.Entities
{
    [HostTypeRequired(HostType.Remote)]
    [NetworkAuthorizationRequired(NetworkAuthorization.Slave)]
    public sealed class PaddleRemoteSlaveComponent : NetworkComponent<Paddle>,
        IMessageProcessor<UserDto>,
        IMessageProcessor<PaddleTargetDto>,
        IMessageFactory<PaddleTargetDto>
    {
        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Entity.Messages.RegisterProcessor<UserDto>(this);
            this.Entity.Messages.RegisterProcessor<PaddleTargetDto>(this);

            this.Entity.Messages.RegisterPacket<PaddleTargetDto, PaddleTargetRequestMessage>(this);
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            this.Entity.Messages.DeregisterProcessor<UserDto>(this);
            this.Entity.Messages.DeregisterProcessor<PaddleTargetDto>(this);

            this.Entity.Messages.DeregisterPacket<PaddleTargetDto, PaddleTargetRequestMessage>(this);
        }
        #endregion

        #region IMessageProcessor Implementations
        void IMessageProcessor<UserDto>.Process(UserDto message)
        {
            if(this.Entity.Pipe.Room.Users.TryGetById(message.UserId, out User user))
            {
                this.Entity.User = user;
            }
        }

        void IMessageProcessor<PaddleTargetDto>.Process(PaddleTargetDto message)
        {
            this.Entity.Target = message.Target;
        }
        #endregion

        #region IMessageFactory Implementations
        public PaddleTargetDto Create()
        {
            return new PaddleTargetDto(this.Entity.Target);
        }
        #endregion
    }
}
