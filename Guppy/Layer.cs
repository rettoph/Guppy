using Guppy.Collections;
using Guppy.Configurations;
using Guppy.Implementations;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    /// <summary>
    /// A layer contains a group of entities,
    /// and will render/update all self held entities
    /// in order based on DrawOrder and UpdateOrder
    /// values. Indavidual layers will be updated and
    /// drawn based on values defined on their configuration.
    /// 
    /// Note, the configuration values are readonly and cannot
    /// be changed once defined.
    /// </summary>
    public abstract class Layer : TrackedDisposable
    {
        #region Protected Internal Attributes
        protected internal LivingObjectCollection<Entity> entities { get; private set; }
        #endregion

        #region Public Attributes
        public readonly LayerConfiguration Configuration;
        #endregion

        #region Constructors
        public Layer(Scene scene, LayerConfiguration configuration)
        {
            this.Configuration = configuration;

            this.entities = new LivingObjectCollection<Entity>();
            this.entities.DisposeOnRemove = false;
        }
        #endregion

        #region Frame Methods
        public abstract void Draw(GameTime gameTime);
        public abstract void Update(GameTime gameTime);
        #endregion
    }
}
