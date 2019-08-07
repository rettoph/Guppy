using Guppy.Collections;
using Guppy.Implementations;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Guppy.Configurations;
using Guppy.Extensions.DependencyInjection;

namespace Guppy
{
    /// <summary>
    /// Scene objects are scoped reusable objects
    /// that contain layers and entities. Entities
    /// should be updated and drawn in their layers
    /// however that functionality can be overwritten
    /// if desired.
    /// </summary>
    public abstract class Scene : Asyncable
    {
        #region Protected Attributes
        protected LayerCollection layers { get; private set; }
        protected EntityCollection entities { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            // Load the layer collection for the scene.
            this.layers = this.provider.GetService<LayerCollection>();
            // Load the entity collection for the scene
            this.entities = this.provider.GetService<EntityCollection>();

            // Update the current scope's scene value
            this.provider.SetConfigurationValue("scene", this);
        }
        #endregion
    }
}
