using Guppy.Collections;
using Guppy.Configurations;
using Guppy.Implementations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Utilities.Cameras;
using Microsoft.Extensions.Logging;

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
    public abstract class Layer : ZFrameable
    {
        #region Protected Internal Attributes
        protected internal ZFrameableCollection<Entity> entities { get; private set; }
        #endregion

        #region Public Attributes
        public readonly LayerConfiguration Configuration;
        public Camera Camera { get; set; }
        #endregion

        #region Constructors
        public Layer(LayerConfiguration configuration, IServiceProvider provider, Camera camera = null)
            : base(provider)
        {
            this.Configuration = configuration;

            this.entities = new ZFrameableCollection<Entity>();
            this.entities.DisposeOnRemove = false;

            this.Camera = camera == null ? provider.GetService<Camera2D>() : camera;
        }
        #endregion

        #region Initialization Methods
        protected override void Boot()
        {
            base.Boot();

            this.SetUpdateOrder(this.Configuration.UpdateOrder);
            this.SetDrawOrder(this.Configuration.DrawOrder);
        }
        #endregion

        #region Frame Methods
        public override void Draw(GameTime gameTime)
        {

        }
        public override void Update(GameTime gameTime)
        {

        }
        #endregion
    }
}
