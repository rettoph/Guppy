using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Configurations;
using Guppy.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.UI.Layers
{
    public class UILayer : Layer
    {
        private GraphicsDevice _graphics;
        private GameWindow _window;

        private Matrix _projection;
        private Matrix _world;
        private Matrix _view;

        private BasicEffect _effect;

        private List<Element> _elements;

        private VertexBuffer _vertexBuffer;
        private Int32 _primitives;

        private SpriteBatch _spriteBatch;

        public Boolean Debug;

        public UILayer(GraphicsDevice graphics, GameWindow window, SpriteBatch spriteBatch, Scene scene, LayerConfiguration configuration) : base(scene, configuration)
        {
            _graphics = graphics;
            _window = window;

            _view = Matrix.Identity;
            _world = Matrix.CreateTranslation(0, 0, 0);

            _effect = new BasicEffect(_graphics);
            _effect.VertexColorEnabled = true;

            _elements = new List<Element>();

            _spriteBatch = spriteBatch;

            this.Debug = false;

            this.entities.Added += this.HandleEntityAdded;
            this.entities.Removed += this.HandleEntityRemoved;
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
            this.entities.Draw(gameTime);
            _spriteBatch.End();

            if (this.Debug && _vertexBuffer != null)
            {
                foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    _graphics.DrawPrimitives(PrimitiveType.LineList, 0, _primitives);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            this.entities.Update(gameTime);

            if (this.Debug)
            {
                _projection = Matrix.CreateOrthographicOffCenter(0, _window.ClientBounds.Width, _window.ClientBounds.Height, 0, 0, 1);
                _effect.Projection = _projection;


                // Load in the vertex data from all contained elements
                List<VertexPositionColor> _allVertices = new List<VertexPositionColor>();

                foreach (Element el in _elements)
                    _allVertices.AddRange(el.GetDebugVertices());

                _vertexBuffer?.Dispose();

                if (_allVertices.Count > 0)
                {
                    _vertexBuffer = new VertexBuffer(_graphics, typeof(VertexPositionColor), _allVertices.Count, BufferUsage.WriteOnly);
                    _vertexBuffer.SetData<VertexPositionColor>(_allVertices.ToArray());

                    _primitives = _allVertices.Count / 2;

                    _graphics.SetVertexBuffer(_vertexBuffer);
                }
                else
                {
                    _vertexBuffer = null;
                }
            }
        }

        #region Event Handlers
        private void HandleEntityRemoved(object sender, Entity e)
        {
            if (e is Element)
                _elements.Remove(e as Element);
        }

        private void HandleEntityAdded(object sender, Entity e)
        {
            if(e is Element)
                _elements.Add(e as Element);
        }
        #endregion
    }
}
