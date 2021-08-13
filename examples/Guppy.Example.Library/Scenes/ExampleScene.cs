using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Lists.Interfaces;
using Guppy.Network.Interfaces;
using Guppy.Network.Peers;
using Guppy.Network.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Example.Library.Scenes
{
    public class ExampleScene : NetworkScene
    {
        protected override void Initialize(GuppyServiceProvider provider)
        {
            base.Initialize(provider);
        }
        protected override IChannel GetChannel(Peer peer)
            => peer.Channels.GetById(0);
    }
}
