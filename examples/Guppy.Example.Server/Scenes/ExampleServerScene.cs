using Guppy.DependencyInjection;
using Guppy.Example.Library;
using Guppy.Example.Library.Scenes;
using Guppy.Lists.Interfaces;
using Guppy.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Example.Server.Scenes
{
    public class ExampleServerScene : ExampleScene
    {
        #region Lifecycle Methods
        protected override void Initialize(GuppyServiceProvider provider)
        {
            base.Initialize(provider);
        }
        #endregion
    }
}
