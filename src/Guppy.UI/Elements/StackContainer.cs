using Guppy.DependencyInjection;
using Guppy.Extensions.System.Collections;
using Guppy.Lists.Interfaces;
using Guppy.UI.Enums;
using Guppy.UI.Interfaces;
using Guppy.UI.Lists;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Elements
{
    /// <summary>
    /// A simple container that can automatically
    /// align its children horizontally or vertically.
    /// 
    /// The container bounds passed to children when
    /// recalculating bounds will be this elements
    /// container.
    /// </summary>
    /// <typeparam name="TChildren"></typeparam>
    public class StackContainer<TChildren> : SecretContainer<TChildren, InnerStackContainer<TChildren>>, IContainer<TChildren>
        where TChildren : class, IElement
    {
        #region Public Properties
        public ElementList<TChildren> Children => this.inner.Children;

        /// <summary>
        /// The current alignment of all internal children.
        /// </summary>
        public StackAlignment Alignment
        {
            get => this.inner.Alignment;
            set => this.inner.Alignment = value;
        }

        public InlineType Inline
        {
            get => this.inline;
            set => this.inline = value;
        }
        #endregion

        #region Helper Methods
        protected override InnerStackContainer<TChildren> BuildContainer(ServiceProvider provider)
            => provider.GetService<InnerStackContainer<TChildren>>((inner, p, c) =>
            {
                inner.Parent = this;
            });
        #endregion
    }

    public class StackContainer : StackContainer<IElement>
    { }

    /// <summary>
    /// The internal container for a <see cref="StackContainer"/> instance.
    /// This is used to contain & render items, but its size is always changing.
    /// </summary>
    /// <typeparam name="TChildren"></typeparam>
    public class InnerStackContainer<TChildren> : Container<TChildren>
        where TChildren : class, IElement
    {
        #region Public Properties
        /// <summary>
        /// The parent stack container that spawned this InnerStackContainer instance.
        /// </summary>
        public StackContainer<TChildren> Parent;

        /// <summary>
        /// The current alignment of all internal children.
        /// </summary>
        public StackAlignment Alignment { get; set; }
        #endregion

        #region Lifecycle Methods
        protected override void Create(ServiceProvider provider)
        {
            base.Create(provider);

            this.OnBoundsCleaned += this.HandleBoundsCleaned;
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.Inline = InlineType.Both;
            this.Bounds.Width = 1f;
            this.Bounds.Height = 1f;

            this.Children.OnAdded += this.HandleChildAdded;
            this.Children.OnRemoved += this.HandleChildRemoved;

            this.AlignChildren();
        }

        protected override void Release()
        {
            base.Release();

            this.Children.OnAdded -= this.HandleChildAdded;
            this.Children.OnRemoved -= this.HandleChildRemoved;
        }

        protected override void Dispose()
        {
            base.Dispose();

            this.OnBoundsCleaned -= this.HandleBoundsCleaned;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Re-align all contained children within the
        /// stack container.
        /// </summary>
        private void AlignChildren()
        {
            if (this.Status != Guppy.Enums.ServiceStatus.Ready)
                return;

            switch (this.Alignment)
            {
                case StackAlignment.Vertical:
                    Int32 y = 0;

                    foreach(TChildren child in this.Children)
                    {
                        child.Bounds.Y = y;
                        y += child.OuterBounds.Height;
                    }
                    break;
                case StackAlignment.Horizontal:
                    Int32 x = 0;

                    foreach (TChildren child in this.Children)
                    {
                        child.Bounds.X = x;
                        x += child.OuterBounds.Width;
                    }
                    break;
            }
        }

        protected override void Refresh()
        {
            base.Refresh();

            this.AlignChildren();
        }
        #endregion

        #region Event Handlers
        private void HandleBoundsCleaned(Element sender)
            => this.AlignChildren();

        private void HandleChildAdded(IServiceList<TChildren> sender, TChildren item)
        {
            item.Bounds.OnSizeChanged += this.HandleChildSizeChanged;

            this.Refresh();
        }

        private void HandleChildRemoved(IServiceList<TChildren> sender, TChildren item)
        {
            item.Bounds.OnSizeChanged -= this.HandleChildSizeChanged;

            this.Refresh();
        }

        private void HandleChildSizeChanged(IElement sender, Bounds bounds)
        {
            this.Refresh();
        }
        #endregion
    }
}
