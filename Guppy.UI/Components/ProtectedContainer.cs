using Guppy.Collections;
using Guppy.Extensions.Collection;
using Guppy.UI.Collections;
using Guppy.UI.Components.Interfaces;
using Guppy.UI.Utilities.Units;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.UI.Components
{
    /// <summary>
    /// Represents an element that is a secret container and does not allow
    /// elements to publicly be added.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public abstract class ProtectedContainer<TElement> : FancyElement
        where TElement : IElement
    {
        #region Protected Fields
        protected ElementCollection<TElement> children;
        #endregion

        #region Lifecycle Method 
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            this.children = ActivatorUtilities.CreateInstance(provider, typeof(ElementCollection<TElement>), this) as ElementCollection<TElement>;
        }

        protected override void PreInitialize()
        {
            base.PreInitialize();

            this.Bounds.Set(0, 0, Unit.Get(1f, -1), Unit.Get(1f, -1));

            this.OnBoundsChanged += this.HandleBoundsChanged;
        }

        public override void Dispose()
        {
            base.Dispose();

            while (this.children.Any())
                this.children.Remove(this.children.First());

            this.OnBoundsChanged -= this.HandleBoundsChanged;
        }
        #endregion

        #region Frame Methods 
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.children.TryUpdateAll(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.children.TryDrawAll(gameTime);
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// When the container bounds are updated we need to clean 
        /// the children.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleBoundsChanged(object sender, Rectangle e)
        {
            this.children.ForEach(c => c.TryClean(true));
        }
        #endregion
    }
}
