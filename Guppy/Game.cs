using Guppy.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Configurations;
using Guppy.Extensions.DependencyInjection;

namespace Guppy
{
    /// <summary>
    /// The main game class for all guppy game instances.
    /// </summary>
    public abstract class Game : Asyncable
    {
        protected override void PreInitialize()
        {
            base.PreInitialize();

            // Update the current scope's game value
            this.provider.SetConfigurationValue("game", this);
        }
    }
}
