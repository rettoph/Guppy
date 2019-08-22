using Guppy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Client.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.Client.Entities
{
    public class FieldEntity : Entity
    {
        private PongScene _scene;

        public FieldEntity(PongScene scene)
        {
            _scene = scene;
        }

        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            this.SetDrawOrder(10);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _scene.LineList.Add(new VertexPositionColor(new Vector3(-1.5f, 1, 0), Color.White));
            _scene.LineList.Add(new VertexPositionColor(new Vector3(1.5f, 1, 0), Color.White));

            _scene.LineList.Add(new VertexPositionColor(new Vector3(-1.5f, -1, 0), Color.White));
            _scene.LineList.Add(new VertexPositionColor(new Vector3(1.5f, -1, 0), Color.White));

            _scene.LineList.Add(new VertexPositionColor(new Vector3(1.5f, -1, 0), Color.White));
            _scene.LineList.Add(new VertexPositionColor(new Vector3(1.5f, 1, 0), Color.White));

            _scene.LineList.Add(new VertexPositionColor(new Vector3(-1.5f, -1, 0), Color.White));
            _scene.LineList.Add(new VertexPositionColor(new Vector3(-1.5f, 1, 0), Color.White));
        }
    }
}
