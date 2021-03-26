using System;
using System.Collections.Generic;
using System.Text;
using Guppy.DependencyInjection;
using Guppy.Network.Interfaces;

namespace Guppy.Network
{
    /// <summary>
    /// A default implementation of a scene capable of creating <see cref="INetworkService"/>
    /// instances automatically based on a defined <see cref="Channel"/> instance's incoming
    /// messages.
    /// </summary>
    public class NetworkScene : Scene
    {
        #region Public Properties
        public IChannel Channel { get; private set; }
        #endregion

        #region Lifecycel Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.Channel.Messages.Add(GuppyNetworkConstants.Messages.NetworkScene.CreateEntity, GuppyNetworkCoreConstants.MessageContexts.InternalReliableDefault);
            this.Channel.Messages.Add(GuppyNetworkConstants.Messages.NetworkScene.UpdateEntity, GuppyNetworkCoreConstants.MessageContexts.InternalReliableDefault);
            this.Channel.Messages.Add(GuppyNetworkConstants.Messages.NetworkScene.RemoveEntity, GuppyNetworkCoreConstants.MessageContexts.InternalReliableDefault);
        }

        protected override void Release()
        {
            base.Release();

            this.Channel.Messages.Remove(GuppyNetworkConstants.Messages.NetworkScene.CreateEntity);
            this.Channel.Messages.Remove(GuppyNetworkConstants.Messages.NetworkScene.UpdateEntity);
            this.Channel.Messages.Remove(GuppyNetworkConstants.Messages.NetworkScene.RemoveEntity);
        }
        #endregion
    }
}
