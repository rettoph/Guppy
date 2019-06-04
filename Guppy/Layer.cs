﻿using Guppy.Collections;
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
        #region Private Fields
        private GraphicsDevice _graphicsDevice;

        private List<VertexPositionColor> _allVertices;
        private VertexBuffer _vertexBuffer;
        private Int32 _primitives;
        private BasicEffect _bassicEffect;
        #endregion

        #region Protected Internal Attributes
        protected internal ZFrameableCollection<Entity> entities { get; private set; }
        #endregion

        #region Public Attributes
        public readonly LayerConfiguration Configuration;
        public Camera Camera { get; set; }
        #endregion

        #region Constructors
        public Layer(LayerConfiguration configuration, IServiceProvider provider, ILogger logger, Camera camera = null)
            : base(provider, logger)
        {
            _graphicsDevice = provider.GetService<GraphicsDevice>();
            _allVertices = new List<VertexPositionColor>();
            _bassicEffect = provider.GetService<BasicEffect>();

            if (_bassicEffect != null)
            {
                _bassicEffect.VertexColorEnabled = true;
            }

            this.Configuration = configuration;

            this.entities = new ZFrameableCollection<Entity>();
            this.entities.DisposeOnRemove = false;

            this.Camera = camera == null ? provider.GetService<Camera2D>() : camera;
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
