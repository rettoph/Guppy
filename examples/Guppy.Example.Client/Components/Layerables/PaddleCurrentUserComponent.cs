using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Example.Library.Components.Layerables;
using Guppy.Example.Library.Layerables;
using Guppy.Example.Library.Messages;
using Guppy.IO.Services;
using Guppy.Network;
using Guppy.Network.Enums;
using Guppy.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Client.Components.Layerables
{
    internal class PaddleCurrentUserComponent : Component<Paddle>
    {
        #region Private Fields
        private IntervalInvoker _intervals;
        private Boolean _currentUser;
        private MouseService _mouse;
        private PositionableRemoteSlaveComponent _remoteSlave;
        private Single _target;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            if(provider.Settings.Get<HostType>() == HostType.Local || provider.GetService<ClientPeer>().CurrentUser == this.Entity.User)
            {
                provider.Service(out _mouse);
                provider.Service(out _intervals);

                _intervals[Library.Constants.Intervals.TargetMessage].OnInterval += this.HandlePaddleTargetMessageInterval;

                _remoteSlave = this.Entity.Components.Get<PositionableRemoteSlaveComponent>();

                this.Entity.OnUpdate += this.Update;

                _currentUser = true;
            }
        }

        protected override void Release()
        {
            base.Release();

            _mouse = default;

            this.Entity.OnUpdate -= this.Update;

            _currentUser = false;
        }
        #endregion

        #region Frame Methods
        private void Update(GameTime gameTime)
        {
            this.Entity.Target = Math.Clamp(_mouse.Position.X, Paddle.Width / 2, Library.Constants.WorldWidth - Paddle.Width / 2) - (Paddle.Width / 2);
        }
        #endregion

        private void HandlePaddleTargetMessageInterval(GameTime gameTime)
        {
            this.Entity.SendMessage<PaddleTargetRequestMessage>();
        }
    }
}
