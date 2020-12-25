using Guppy.DependencyInjection;
using Guppy.Events.Delegates;
using Guppy.Extensions.DependencyInjection;
using Guppy.Extensions.System;
using Guppy.Extensions.Utilities;
using Guppy.IO.Enums;
using Guppy.IO.Services;
using Guppy.UI.Delegates;
using Guppy.UI.Enums;
using Guppy.UI.Extensions.Microsoft.Xna.Framework.Graphics;
using Guppy.UI.Interfaces;
using Guppy.UI.Services;
using Guppy.UI.Utilities;
using Guppy.Utilities;
using Guppy.Utilities.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private PrimitiveBatch<VertexPositionColor> _primitiveBatch;
        private Queue<IDisposable> _stateValues;
        private PrimitivePath _border;
        private GraphicsDevice _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _background;
        private SpriteFont _font;
        private IContainer _container;
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

        /// <inheritdoc />
        public IContainer Container
        {
            get => _container;
            set => this.OnContainerChanged.InvokeIf(value != _container, this, ref _container, value);
        }

        /// <summary>
        /// The current background color, if any
        /// </summary>
        public ElementStateValue<Color> BackgroundColor { get; private set; }

        /// <summary>
        /// The current border color, if any
        /// </summary>
        public ElementStateValue<Color> BorderColor { get; private set; }

        /// <summary>
        /// The current border width, if any.
        /// </summary>
        public ElementStateValue<Single> BorderWidth { get; private set; }

        /// <summary>
        /// The current border width, if any.
        /// </summary>
        public ElementStateValue<Texture2D> BackgroundImage { get; private set; }
        #endregion

        #region Events
        /// <inheritdoc />
        public event OnStateChangedDelegate OnStateChanged;

        /// <inheritdoc />
        public Dictionary<ElementState, OnStateChangedDelegate> OnState { get; private set; }

        public OnEventDelegate<Element> OnBoundsCleaned;

        /// <summary>
        /// Invoked when the mouse is released while
        /// the element is hovered.
        /// </summary>
        public event OnEventDelegate<Element> OnClicked;

        /// <summary>
        /// Invoked when the parent is updated. When created,
        /// this process automatically takes place after pre-initialization but
        /// before initialization. Unfortunately that means this should 
        /// be used when setting up any automated internal fields.
        /// </summary>

        public event OnChangedEventDelegate<Element, IContainer> OnContainerChanged;
        #endregion

        #region Lifecycle Methods
        protected override void Create(ServiceProvider provider)
        {
            base.Create(provider);

            _font = provider.GetContent<SpriteFont>("ui:font");

            _stateValues = new Queue<IDisposable>();

            this.OnState = DictionaryHelper.BuildEnumDictionary<ElementState, OnStateChangedDelegate>();

            this.OnBoundsCleaned += this.HandleBoundsCleaned;
        }

        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _ui);
            provider.Service(out _primitiveBatch);
            provider.Service(out _spriteBatch);
            provider.Service(out _graphics);

            _background = _graphics.BuildPixel();

            this.Padding = new Padding(this);
            this.Bounds = new Bounds(this);

            this.Padding.OnChanged += this.HandlePaddingChanged;

            this.OnStateChanged += this.HandleStateChanged;
            this.OnState[ElementState.Hovered] += this.HandleHoveredStateChanged;
            this.OnState[ElementState.Pressed] += this.HandlePressedStateChanged;

            this.BackgroundColor = this.BuildStateValue<Color>();
            this.BorderColor = this.BuildStateValue<Color>();
            this.BorderWidth = this.BuildStateValue<Single>();
            this.BackgroundImage = this.BuildStateValue<Texture2D>();
        }

        protected override void Release()
        {
            base.Release();

            this.Padding.OnChanged -= this.HandlePaddingChanged;

            while (_stateValues.Any())
                _stateValues.Dequeue().Dispose();

            // Remove the container reference.
            this.Container = null;
        }

        protected override void Dispose()
        {
            base.Dispose();

            this.OnBoundsCleaned -= this.HandleBoundsCleaned;
        }
        #endregion

        #region Frame Methods
        protected override void PreDraw(GameTime gameTime)
        {
            base.PreDraw(gameTime);

            _graphics.PushScissorRectangle(this.OuterBounds);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (this.BackgroundColor != default(Color)) 
            {
                _spriteBatch.Draw(_background, this.OuterBounds, this.BackgroundColor);
            }
                

            if (this.BorderColor != default(Color) && this.BorderWidth > 0)
            {
                _border.Width = this.BorderWidth;
                _primitiveBatch.DrawPrimitive(_border, this.BorderColor);
            }

            if(this.BackgroundImage.Current != default(Texture2D))
            {
                _spriteBatch.Draw(this.BackgroundImage, this.OuterBounds, Color.White);
            }

            // _primitiveBatch.TraceRectangle(Color.Blue, this.InnerBounds);
            // _primitiveBatch.TraceRectangle(Color.Red, this.OuterBounds);
            // _spriteBatch.DrawString(_font, $"{this.GetType().GetPrettyName()}<{this.ServiceConfiguration.Name}>({this.Id})", this.OuterBounds.Location.ToVector2(), Color.White);
        }

        protected override void PostDraw(GameTime gameTime)
        {
            base.PostDraw(gameTime);

            _graphics.PopScissorRectangle();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        #endregion

        #region Methods
        /// <inheritdoc />
        public void TryCleanBounds()
        {
            this.TryCleanOuterBounds(this.Container.GetInnerBoundsForChildren());
            this.TryCleanInnerBounds();

            this.OnBoundsCleaned?.Invoke(this);
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
            {
                this.TrySetState(ElementState.Focused, true);
                this.OnClicked?.Invoke(this);
            }
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

        /// <summary>
        /// When the bounds are cleaned we should just recalculate
        /// the border primitives.
        /// </summary>
        /// <param name="sender"></param>
        private void HandleBoundsCleaned(Element sender)
        {
            _border = PrimitivePath.Create(
                1f,
                new Vector2(this.OuterBounds.Left, this.OuterBounds.Top),
                new Vector2(this.OuterBounds.Right, this.OuterBounds.Top),
                new Vector2(this.OuterBounds.Right, this.OuterBounds.Bottom),
                new Vector2(this.OuterBounds.Left, this.OuterBounds.Bottom));
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
