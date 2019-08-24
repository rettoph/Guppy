using Guppy.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Collections;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Guppy
{
    public class Layer : Driven
    {
        #region Internal Attributes
        internal FrameableCollection<Entity> entities { get; private set; }
        #endregion

        #region Public Attributes
        /// <summary>
        /// A list of all enabled entities withing the layer ordered by UpdateOrder.
        /// </summary>
        public IEnumerable<Entity> Updates { get { return this.entities.Updates; } }
        /// <summary>
        /// A list of all visible entities within the layer ordered by DrawOrder.
        /// </summary>
        public IEnumerable<Entity> Draws { get { return this.entities.Draws; } }

        /// <summary>
        /// A list of all entities, including enabled and disabled.
        /// </summary>
        public IEnumerable<Entity> Entities { get { return this.entities.AsEnumerable(); } }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            this.entities = provider.GetService<FrameableCollection<Entity>>();
        }
        #endregion

        #region Frame Methods
        protected override void Update(GameTime gameTime)
        {
            this.entities.TryCleanUpdates();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.entities.TryCleanDraws();

            base.Draw(gameTime);
        }
        #endregion
    }
}
