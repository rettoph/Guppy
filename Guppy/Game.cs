using Guppy.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy
{
    /// <summary>
    /// Games are creatable singletons.
    /// 
    /// Ther ecan only be one game per service provider,
    /// and the GuppyLoader.BuildGame method should be used
    /// to create your game instance.
    /// </summary>
    public class Game : Asyncable
    {
        #region Protected Attributes
        protected SceneCollection scenes { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            this.scenes = provider.GetService<SceneCollection>();
        }
        #endregion
    }
}
