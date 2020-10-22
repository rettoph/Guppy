
using Guppy.DependencyInjection;
using Guppy.Events.Delegates;
using Guppy.Extensions.Collections;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Lists;
using Guppy.Lists.Interfaces;
using Guppy.UI.Enums;
using Guppy.UI.Interfaces;
using Guppy.UI.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.UI.Elements
{
    public class Container<TChildren> : Element, IContainer<TChildren>
        where TChildren : class, IElement
    {
        #region Public Properties
        public ServiceList<TChildren> Children { get; private set; }
        #endregion

        #region Events
        private event OnEventDelegate<IElement, GameTime> OnUpdateChild;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            // Create & setup a new children list.
            this.Children = provider.GetService<ServiceList<TChildren>>();
            this.Children.OnAdded += this.HandleChildAdded;
            this.Children.OnRemoved += this.HandleChildRemoved;

            this.OnUpdateChild += this.UpdateChild;
            this.OnState[ElementState.Hovered] += this.HandleHoveredStateChanged;
        }

        protected override void Release()
        {
            base.Release();

            this.Children.ForEach(c => c.TryRelease());
            this.Children.OnAdded -= this.HandleChildAdded;
            this.Children.OnRemoved -= this.HandleChildRemoved;
            this.Children.TryRelease();

            this.OnUpdateChild -= this.UpdateChild;
            this.OnState[ElementState.Hovered] -= this.HandleHoveredStateChanged;
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.Children.TryDrawAll(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.Children.ForEach(c => this.OnUpdateChild.Invoke(c, gameTime));
        }
        #endregion

        #region Methods
        protected override bool TryCleanInnerBounds()
        {
            if(base.TryCleanInnerBounds())
            {
                // Recursively clean all children.
                this.Children.ForEach(c => c.TryCleanBounds(this.InnerBounds));

                return true;
            }

            return false;
        }
        #endregion

        #region Child Update Methods
        private void UpdateChild(IElement sender, GameTime arg)
            => sender.TryUpdate(arg);

        private void TryCleanChildHovered(IElement sender, GameTime arg)
            => sender.TryCleanHovered();
        #endregion

        #region Event Handlers
        private void HandleChildAdded(IServiceList<TChildren> sender, TChildren child)
        {
            child.Bounds.OnChanged += this.HandleChildBoundsChanged;
        }

        private void HandleChildRemoved(IServiceList<TChildren> sender, TChildren child)
        {
            child.Bounds.OnChanged -= this.HandleChildBoundsChanged;
        }

        private void HandleChildBoundsChanged(IElement sender, Bounds bounds)
            => sender.TryCleanBounds(this.InnerBounds);

        private void HandleHoveredStateChanged(IElement sender, ElementState which, bool value)
        {
            if (value)
                this.OnUpdateChild += this.TryCleanChildHovered;
            else
                this.OnUpdateChild -= this.TryCleanChildHovered;
        }
        #endregion
    }
}
