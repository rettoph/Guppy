using Guppy.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Configurations;

namespace Guppy
{
    /// <summary>
    /// The main game class for all guppy game instances.
    /// </summary>
    public abstract class Game : Driven
    {
        protected override void PreInitialize()
        {
            base.PreInitialize();

            // Update the current scope's game value
            var config = this.provider.GetService<ScopeConfiguration>();
            config.Set("game", this);
        }
    }
}
