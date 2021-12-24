using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.Interfaces;
using Guppy.Utilities;
using Guppy.Utilities.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Examples.Client.Components
{
    public abstract class DrawComponent<TEntity> : Component<TEntity>
        where TEntity : Frameable
    {
        #region Protected Properties
        protected PrimitiveBatch<VertexPositionColor> primitiveBatch { get; private set; }
        protected Camera2D camera { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.primitiveBatch = provider.GetService<PrimitiveBatch<VertexPositionColor>>();
            this.camera = provider.GetService<Camera2D>();

            this.Entity.OnDraw += this.Draw;
        }
        #endregion

        #region Frame Methods
        protected abstract void Draw(GameTime gameTime);
        #endregion
    }
}
