using Guppy.Collections;
using Guppy.Configurations;
using Guppy.Implementations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        #region Private Fields
        private GameWindow _window;
        private GraphicsDevice _graphicsDevice;
        private List<DebuggableEntity> _debuggables;

        private Matrix _projection;
        private Matrix _world;
        private Matrix _view;

        private BasicEffect _effect;

        private List<VertexPositionColor> _allVertices;
        private VertexBuffer _vertexBuffer;
        private Int32 _primitives;

        private Boolean _dirtyBounds;
        #endregion

        #region Protected Internal Attributes
        protected internal LivingObjectCollection<Entity> entities { get; private set; }
        #endregion

        #region Public Attributes
        public readonly LayerConfiguration Configuration;
        public Boolean Debug { get; set; }
        #endregion

        #region Constructors
        public Layer(Scene scene, LayerConfiguration configuration, GameWindow window = null, GraphicsDevice graphicsDevice = null)
        {
            _debuggables = new List<DebuggableEntity>();

            this.Configuration = configuration;
            this.Debug = true;

            this.entities = new LivingObjectCollection<Entity>();
            this.entities.DisposeOnRemove = false;

            this.entities.Added += this.HandleEntityAdded;
            this.entities.Removed += this.HandleEntityRemoved;

            
            if(window != null && graphicsDevice != null) {
                _window = window;
                _graphicsDevice = graphicsDevice;

                // Setup debug view values...
                _view = Matrix.Identity;
                _world = Matrix.CreateTranslation(0, 0, 0);
                _allVertices = new List<VertexPositionColor>();

                _effect = new BasicEffect(_graphicsDevice);
                _effect.VertexColorEnabled = true;

                _dirtyBounds = true;

                _window.ClientSizeChanged += this.HandleClientSizeChanged;
            }
        }
        #endregion

        #region Frame Methods
        public abstract void Draw(GameTime gameTime);
        public abstract void Update(GameTime gameTime);
        public void DebugDraw(GameTime gameTime)
        {
            if(_dirtyBounds)
            {
                // Update debug projection settings...
                _projection = Matrix.CreateOrthographicOffCenter(0, _window.ClientBounds.Width, _window.ClientBounds.Height, 0, 0, 1);
                _effect.Projection = _projection;
            }

            if (this.Debug)
            { // Draw the debug overlay...
                _allVertices.Clear();
                _vertexBuffer?.Dispose();

                foreach (DebuggableEntity de in _debuggables)
                    de.AddDebugVertices(ref _allVertices);

                // Calculate the primitive total
                _primitives = _allVertices.Count / 2;

                if (_primitives > 0)
                {
                    _vertexBuffer = new VertexBuffer(_graphicsDevice, typeof(VertexPositionColor), _allVertices.Count, BufferUsage.WriteOnly);
                    _vertexBuffer.SetData<VertexPositionColor>(_allVertices.ToArray());
                    _graphicsDevice.SetVertexBuffer(_vertexBuffer);

                    foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        _graphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, _primitives);
                    }
                }
            }
        }
        #endregion

        #region Event Handlers 
        private void HandleEntityRemoved(object sender, Entity e)
        {
            if (e is DebuggableEntity)
                _debuggables.Remove(e as DebuggableEntity);
        }

        private void HandleEntityAdded(object sender, Entity e)
        {
            if (e is DebuggableEntity)
                _debuggables.Add(e as DebuggableEntity);
        }

        private void HandleClientSizeChanged(object sender, EventArgs e)
        {
            _dirtyBounds = true;
        }
        #endregion
    }
}
