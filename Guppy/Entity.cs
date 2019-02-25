using Guppy.Implementations;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public class Entity : LivingObject
    {
        #region Protected Attributes
        protected Scene scene { get; private set; }
        #endregion

        #region Public Attributes
        public UInt16 LayerDepth { get; private set; }
        #endregion

        #region Events
        public event EventHandler<Entity> OnLayerDepthChanged;
        #endregion

        #region Constructors
        public Entity(Scene scene, ILogger logger) : base(logger)
        {
            this.scene = scene; // Save the entities scene
            this.SetLayerDepth(this.scene.DefaultLayerDepth); // Set the initial layer depth to the default layer depth
        }
        #endregion

        #region Frame Methods
        public override void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Methods 
        public void SetLayerDepth(UInt16 layerDepth)
        {
            this.LayerDepth = layerDepth;

            this.OnLayerDepthChanged?.Invoke(this, this);
        }
        #endregion
    }
}
