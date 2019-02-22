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
    public class Layer : TrackedDisposable
    {
        #region Protected Attributes
        #endregion

        #region Public Attributes
        public readonly LayerConfiguration Configuration;
        #endregion

        #region Constructors
        public Layer(Scene scene, LayerConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        #endregion

        #region Frame Methods
        public virtual void Draw(GameTime gameTime)
        {
            // throw new NotImplementedException();
        }

        public virtual void Update(GameTime gameTime)
        {
            // throw new NotImplementedException();
        }
        #endregion
    }
}
