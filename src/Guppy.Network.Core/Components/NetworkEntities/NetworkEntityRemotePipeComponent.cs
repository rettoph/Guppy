using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Attributes;
using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Components.NetworkEntities
{
    /// <summary>
    /// Automatically add a <see cref="INetworkEntity"/> into the given pipe 
    /// when the event is triggered.
    /// </summary>
    [HostTypeRequired(HostType.Remote)]
    internal sealed class NetworkEntityRemotePipeComponent : NetworkComponent<INetworkEntity>
    {
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Entity.OnPipeChanged += this.HandlePipeChanged;

            this.CleanPipe(default, this.Entity.Pipe);
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            this.Entity.Pipe = default;

            this.Entity.OnPipeChanged -= this.HandlePipeChanged;
        }

        private void CleanPipe(Pipe old, Pipe value)
        {
            // Alert the old pipe to remove the entity...
            old?.RemoveNetworkEntity(this.Entity, value);
            // Add the entity to the new pipe...
            value?.TryAddNetworkEntity(this.Entity, old);
        }

        private void HandlePipeChanged(INetworkEntity sender, Pipe old, Pipe value)
        {
            this.CleanPipe(old, value);
        }
    }
}
