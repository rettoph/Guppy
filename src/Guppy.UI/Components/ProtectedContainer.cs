using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.UI.Collections;
using Guppy.UI.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.UI.Components
{
    /// <summary>
    /// A private implementation of a container. This dows not contain
    /// public children accessors and should primarily be used for objects
    /// that contain internal children accessible from the outside.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    public class ProtectedContainer<TComponent> : Component, IBaseContainer
        where TComponent : IComponent
    {
        #region Protected Attributes
        protected ComponentCollection<TComponent> children { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.children = provider.GetService<ComponentCollection<TComponent>>();
            this.children.Parent = this;
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.DrawChildren(gameTime);
        }

        protected virtual void DrawChildren(GameTime gameTime)
        {
            this.children.ForEach(c => c.TryDraw(gameTime));
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.UpdateChildren(gameTime);
        }

        protected virtual void UpdateChildren(GameTime gameTime)
        {
            this.children.ForEach(c => c.TryUpdate(gameTime));
        }
        #endregion

        #region IBaseContainer Implementation 
        /// <inheritdoc />
        Point IBaseContainer.GetContainerLocation()
            => this.GetContainerLocation();

        /// <summary>
        /// Return the location of the container that all 
        /// children should utilize for positioning.
        /// </summary>
        /// <returns></returns>
        protected virtual Point GetContainerLocation()
        {
            return this.Bounds.Pixel.Location;
        }

        /// <inheritdoc />
        Point IBaseContainer.GetContainerSize()
            => this.GetContainerSize();

        /// <summary>
        /// Return the size of the container that all 
        /// children should utilize for sizing.
        /// </summary>
        /// <returns></returns>
        protected virtual Point GetContainerSize()
        {
            return this.Bounds.Pixel.Size;
        }
        #endregion
    }
}
