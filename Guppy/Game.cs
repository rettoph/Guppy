using Guppy.Collections;
using Guppy.Implementations;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy
{
    public class Game : Asyncable
    {
        #region Protected Fields
        protected SceneCollection scenes { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            // Load an easily referenceable scene collection
            this.scenes = provider.GetService<SceneCollection>();
        }
        #endregion
    }
}
