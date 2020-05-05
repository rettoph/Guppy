using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.UI.Collections;
using Guppy.UI.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Components
{
    /// <summary>
    /// A private implementation of a container. This dows not contain
    /// public children accessors and should primarily be used for objects
    /// that contain internal children accessible from the outside.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    public class ProtectedContainer : Component
    {
        #region Protected Attributes
        protected ComponentCollection children { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.children = provider.GetService<ComponentCollection>();
            this.children.Parent = this;
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.children.ForEach(c => c.TryDraw(gameTime));
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.children.ForEach(c => c.TryUpdate(gameTime));
        }
        #endregion
    }
}
