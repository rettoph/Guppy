using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.Lists.Interfaces;
using Guppy.UI.Enums;
using Guppy.UI.Interfaces;
using Guppy.UI.Lists;
using Guppy.UI.Utilities;
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
    public class StackContainer<TChildren> : Element, IContainer<TChildren>
        where TChildren : class, IElement
    {
        #region Private Fields
        private InnerStackContainer<TChildren> _inner;
        #endregion

        #region Public Properties
        public ElementList<TChildren> Children => _inner.Children;

        /// <summary>
        /// The current alignment of all internal children.
        /// </summary>
        public StackAlignment Alignment
        {
            get => _inner.Alignment;
            set => _inner.Alignment = value;
        }
        #endregion

        #region Lifecycle Methods
        protected override void Create(ServiceProvider provider)
        {
            base.Create(provider);

            this.OnBoundsCleaned += this.HandleBoundsCleaned;
        }
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _inner, (inner, p, c) =>
            {
                inner.Parent = this;
            });
        }

        protected override void Dispose()
        {
            base.Dispose();

            this.OnBoundsCleaned -= this.HandleBoundsCleaned;
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _inner.TryDraw(gameTime);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _inner.TryUpdate(gameTime);
        }
        #endregion

        #region Events
        private void HandleBoundsCleaned(Element sender)
            => _inner.TryCleanBounds(this.InnerBounds);
        #endregion
    }

    public class StackContainer : StackContainer<IElement>
    { }

    /// <summary>
    /// The internal container for a <see cref="StackContainer"/> instance.
    /// This is used to contain & render items, but its size is always changing.
    /// </summary>
    /// <typeparam name="TChildren"></typeparam>
    internal class InnerStackContainer<TChildren> : Container<TChildren>
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
        /// <inheritdoc />
        protected override Rectangle GetInnerBoundsForChildren()
            => this.Parent.InnerBounds;

        /// <summary>
        /// Re-align all contained children within the
        /// stack container.
        /// </summary>
        private void AlignChildren()
        {
            switch (this.Alignment)
            {
                case StackAlignment.Vertical:
                    Int32 y = 0;
                    Int32 width = 0;

                    this.Children.ForEach(c =>
                    {
                        c.Bounds.Y = y;
                        y += c.OuterBounds.Height;
                        width = Math.Max(c.OuterBounds.Width, width);
                    });

                    this.Parent.Bounds.Height = y;
                    this.Parent.Bounds.Width = width;
                    break;
                case StackAlignment.Horizontal:
                    Int32 x = 0;
                    Int32 height = 0;

                    this.Children.ForEach(c =>
                    {
                        c.Bounds.X = x;
                        x += c.OuterBounds.Width;
                        height = Math.Max(c.OuterBounds.Height, height);
                    });

                    this.Parent.Bounds.Width = x;
                    this.Parent.Bounds.Height = height;
                    break;
            }
        }
        #endregion

        #region Event Handlers
        private void HandleBoundsCleaned(Element sender)
            => this.AlignChildren();

        private void HandleChildAdded(IServiceList<TChildren> sender, TChildren item)
        {
            item.Bounds.OnSizeChanged += this.HandleChildSizeChanged;

            this.AlignChildren();
        }

        private void HandleChildRemoved(IServiceList<TChildren> sender, TChildren item)
        {
            item.Bounds.OnSizeChanged -= this.HandleChildSizeChanged;

            this.AlignChildren();
        }

        private void HandleChildSizeChanged(IElement sender, Bounds bounds)
            => this.AlignChildren();
        #endregion
    }
}
