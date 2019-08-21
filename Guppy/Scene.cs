using Guppy.Implementations;
using Guppy.Utilities.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Collections;
using Guppy.Attributes;

namespace Guppy
{
    [IsScene]
    public class Scene : Asyncable
    {
        #region Protected Attributes
        protected LayerCollection layers { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            this.layers = provider.GetService<LayerCollection>();
        }
        #endregion
    }
}
