using Guppy.DependencyInjection;
using Guppy.Network.Attributes;
using Guppy.Network.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Components.Scenes
{
    [NetworkAuthorizationRequired(NetworkAuthorization.Master)]
    internal sealed class NetworkSceneMasterCRUDComponent : NetworkSceneBaseCRUDComponent
    {
        protected override void Initialize(GuppyServiceProvider provider)
        {
            base.Initialize(provider);
        }
    }
}
