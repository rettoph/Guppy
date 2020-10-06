using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Extensions.Utilities;
using Guppy.IO.Enums;
using Guppy.IO.Input.Helpers;
using Guppy.IO.Services;
using Guppy.UI.Delegates;
using Guppy.UI.Enums;
using Guppy.UI.Interfaces;
using Guppy.UI.Services;
using Guppy.UI.Utilities;
using Guppy.Utilities;
using Guppy.Utilities.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.UI.Elements
{
    /// <summary>
    /// A basic implementation of the IElement interface...
    /// </summary>
    public class Element : Frameable, IElement
    {
        #region Private Fields
        private UIService _ui;
        private PrimitiveBatch _primitiveBatch;
        private Queue<IDisposable> _stateValues;
        #endregion

        #region Public Properties
        /// <inheritdoc />
        public ElementState State { get; private set; }

        /// <inheritdoc />
        public Bounds Bounds { get; private set; }

        /// <inheritdoc />
        public Padding Padding { get; private set; }

        /// <inheritdoc />
        public Rectangle OuterBounds { get; private set; }

        /// <inheritdoc />
        public Rectangle InnerBounds { get; private set; }

        /// <summary>
        /// The current background color, if any
        /// </summary>
        public ElementStateValue<Color> BackgroundColor { get; private set; }

        /// <summary>
        /// The current border color, if any
        /// </summary>
        public ElementStateValue<Color> BorderColor { get; private set; }
        #endregion

        #region Events
        /// <inheritdoc />
        public event OnStateChangedDelegate OnStateChanged;

        /// <inheritdoc />
        public Dictionary<ElementState, OnStateChangedDelegate> OnState { get; private set; }
        #endregion

        #region Lifecycle Methods
        protected override void Create(ServiceProvider provider)
        {
            base.Create(provider);

            _stateValues = new Queue<IDisposable>();

            this.OnState = DictionaryHelper.BuildEnumDictionary<ElementState, OnStateChangedDelegate>();
        }

        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _ui);
            provider.Service(out _primitiveBatch);

            this.Padding = new Padding(this);
            this.Bounds = new Bounds(this);

            this.Padding.OnChanged += this.HandlePaddingChanged;

            this.OnStateChanged += this.HandleStateChanged;
            this.OnState[ElementState.Hovered] += this.HandleHoveredStateChanged;
            this.OnState[ElementState.Pressed] += this.HandlePressedStateChanged;

            this.BackgroundColor = this.BuildStateValue<Color>();
            this.BorderColor = this.BuildStateValue<Color>();
        }

        protected override void Release()
        {
            base.Release();

            this.Padding.OnChanged -= this.HandlePaddingChanged;

            while (_stateValues.Any())
                _stateValues.Dequeue().Dispose();
        }
        #endregion

        #region Frame Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // if (this.BackgroundColor.Current != default(Color))
            //     _primitiveBatch.FillRectangle(this.OuterBounds, this.BackgroundColor.Current);
            // 
            // if (this.BorderColor.Current != default(Color))
            //     _primitiveBatch.DrawRectangle(this.OuterBounds, this.BorderColor.Current);

            // _primitiveBatch.DrawRectangle(this.InnerBounds, Color.Blue);
            // _primitiveBatch.DrawRectangle(this.OuterBounds, Color.Red);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        #endregion

        #region Methods
        /// <inheritdoc />
        public void TryCleanBounds(Rectangle container)
        {
            this.TryCleanOuterBounds(container);
            this.TryCleanInnerBounds();
        }

        /// <summary>
        /// Recalculate the outer bounds and if its changed return true.
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        protected virtual Boolean TryCleanOuterBounds(Rectangle container)
            => this.OuterBounds != (this.OuterBounds = this.CalculateOuterBounds(container));

        /// <summary>
        /// Calculate the outer bounds based on a recieved 
        /// container bounds.
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        protected virtual Rectangle CalculateOuterBounds(Rectangle container)
            => new Rectangle()
            {
                X = container.X + this.Bounds.X.ToPixel(container.Width),
                Y = container.Y + this.Bounds.Y.ToPixel(container.Height),
                Width = this.Bounds.Width.ToPixel(container.Width),
                Height = this.Bounds.Height.ToPixel(container.Height)
            };

        /// <summary>
        /// Recalculate the inner bounds and if its changed return true.
        /// </summary>
        /// <returns></returns>
        protected virtual Boolean TryCleanInnerBounds()
            => this.InnerBounds != (this.InnerBounds = this.CalculateInnerBounds());

        /// <summary>
        /// Calculate the inner bounds based on the current
        /// outer bounds.
        /// </summary>
        /// <returns></returns>
        protected virtual Rectangle CalculateInnerBounds()
            => new Rectangle()
            {
                X = this.OuterBounds.X + this.Padding.Left.ToPixel(this.InnerBounds.Width),
                Y = this.OuterBounds.Y + this.Padding.Top.ToPixel(this.InnerBounds.Height),
                Width = this.OuterBounds.Width - this.Padding.Left.ToPixel(this.InnerBounds.Width) - this.Padding.Right.ToPixel(this.InnerBounds.Width),
                Height = this.OuterBounds.Height - this.Padding.Top.ToPixel(this.InnerBounds.Height) - this.Padding.Bottom.ToPixel(this.InnerBounds.Height)
            };

        protected void TrySetState(ElementState state, Boolean value)
        {
            if(this.State.HasFlag(state) && !value)
            {
                this.State &= ~state;

                this.OnStateChanged?.Invoke(this, state, value);
                this.OnState[state]?.Invoke(this, state, value);
            }
            else if(!this.State.HasFlag(state) && value)
            {
                this.State |= state;

                this.OnStateChanged?.Invoke(this, state, value);
                this.OnState[state]?.Invoke(this, state, value);
            }
        }

        /// <inheritdoc />
        public void TryCleanHovered()
            => this.TrySetState(ElementState.Hovered, this.OuterBounds.Contains(_ui.Target));
        #endregion

        #region Event Handlers
        private void HandlePaddingChanged(IElement sender, Padding padding)
            => this.TryCleanInnerBounds();

        private void HandleHoveredStateChanged(IElement sender, ElementState which, bool value)
        {
            if(value)
            { // Begin listening for pressed events now!
                _ui.OnPressedChanged += this.HandleUIPressedChanged;
            }
        }

        private void HandlePressedStateChanged(IElement sender, ElementState which, bool value)
        {
            if (value && !this.State.HasFlag(ElementState.Hovered))
                this.TrySetState(ElementState.Focused, false);
            else if (!value && this.State.HasFlag(ElementState.Hovered))
                this.TrySetState(ElementState.Focused, true);
        }

        private void HandleStateChanged(IElement sender, ElementState which, bool value)
        {
            if (this.State == ElementState.Default)
            { // Only stop listening to pressed events if the element state is "empty"
                _ui.OnPressedChanged -= this.HandleUIPressedChanged;
            }
        }

        /// <summary>
        /// Set the current pressed value of the element
        /// based on the global UI pressed event manager.
        /// </summary>
        /// <param name="value"></param>
        private void HandleUIPressedChanged(Boolean value)
        {
            if (value)
            {
                if (this.State.HasFlag(ElementState.Hovered))
                    this.TrySetState(ElementState.Pressed, true);
                else
                    this.TrySetState(ElementState.Focused, false);
            }
            else
                this.TrySetState(ElementState.Pressed, false);
        }

        protected ElementStateValue<T> BuildStateValue<T>(T defaultValue = default(T))
        {
            var sv = new ElementStateValue<T>(this, defaultValue);

            _stateValues.Enqueue(sv);

            return sv;
        }
        #endregion
    }
}
