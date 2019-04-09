using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Guppy.UI.Entities
{
    public class InputManager : DebuggableEntity
    {
        private Vector2 _deltaPosition;
        public Vector2 DeltaPosition { get { return _deltaPosition; } }

        private Vector2 _position;
        public Vector2 Position { get { return _position; } }

        public Boolean Down { get; private set; }

        public InputManager(EntityConfiguration configuration, Scene scene, ILogger logger) : base(configuration, scene, logger)
        {
            _deltaPosition = new Vector2(0, 0);
            _position = new Vector2(0, 0);

            this.Down = false;

            this.SetVisible(false);
        }

        public override void AddDebugVertices(ref List<VertexPositionColor> vertices)
        {
            // Delta Trail
            vertices.Add(new VertexPositionColor(new Vector3(_position.X, _position.Y, 0), Color.Yellow));
            vertices.Add(new VertexPositionColor(new Vector3(_position.X - _deltaPosition.X, _position.Y - _deltaPosition.Y, 0), Color.Yellow));

            // Position Crosshair
            var color = this.Down ? Color.Green : Color.Red;
            vertices.Add(new VertexPositionColor(new Vector3(_position.X - 5, _position.Y, 0), color));
            vertices.Add(new VertexPositionColor(new Vector3(_position.X, _position.Y - 5, 0), color));

            vertices.Add(new VertexPositionColor(new Vector3(_position.X, _position.Y - 5, 0), color));
            vertices.Add(new VertexPositionColor(new Vector3(_position.X + 5, _position.Y, 0), color));

            vertices.Add(new VertexPositionColor(new Vector3(_position.X + 5, _position.Y, 0), color));
            vertices.Add(new VertexPositionColor(new Vector3(_position.X, _position.Y + 5, 0), color));

            vertices.Add(new VertexPositionColor(new Vector3(_position.X, _position.Y + 5, 0), color));
            vertices.Add(new VertexPositionColor(new Vector3(_position.X - 5, _position.Y, 0), color));

            vertices.Add(new VertexPositionColor(new Vector3(_position.X + 5, _position.Y, 0), color));
            vertices.Add(new VertexPositionColor(new Vector3(_position.X - 5, _position.Y, 0), color));

            vertices.Add(new VertexPositionColor(new Vector3(_position.X, _position.Y - 5, 0), color));
            vertices.Add(new VertexPositionColor(new Vector3(_position.X, _position.Y + 5, 0), color));
        }

        public override void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            var mState = Mouse.GetState();

            // Update the mouse's delta position
            _deltaPosition.X = mState.Position.X - _position.X;
            _deltaPosition.Y = mState.Position.Y - _position.Y;

            // Update the mouse's current position
            _position.X = mState.Position.X;
            _position.Y = mState.Position.Y;

            // Update the mouse's left button down state
            this.Down = mState.LeftButton == ButtonState.Pressed;
        }
    }
}
