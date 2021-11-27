using Guppy.DependencyInjection;
using Guppy.Example.Library;
using Guppy.Lists.Interfaces;
using Guppy.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.Example.Server
{
    public class ExampleServerGame : ExampleGame
    {
        #region Lifecycle Methods
        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);
        }
        #endregion
    }
}
