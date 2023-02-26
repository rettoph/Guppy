using Guppy.MonoGame.UI.Providers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Elements
{
    public class Element : IElement
    {
        private IStyleProvider _styles;
        private Rectangle _outerBounds;
        private Rectangle _innerBounds;

        protected IContainer container { get; private set; }

        public Selector Selector { get; }
        public IStyleProvider Styles => _styles;
        public ElementState State { get; protected set; }

        public Rectangle OuterBounds => _outerBounds;
        public Rectangle InnerBounds => _innerBounds;

        public StyleValue<Display> DisplayHorizontal { get; private set; }
        public StyleValue<Display> DisplayVertical { get; private set; }

        public Element(params string[] names)
        {
            _styles = null!;

            this.container = null!;

            this.Selector = new Selector(this.GetType(), names);

            this.DisplayHorizontal = null!;
            this.DisplayVertical = null!;
        }

        protected virtual void Initialize(IContainer container)
        {
            this.container = container;

            this.Initialize(container, out _styles, out Selector? parent);
            this.Selector.Parent = parent;

            this.State |= ElementState.Initialized;
        }

        protected virtual void InitializeStyles(IStyleProvider styles)
        {
            this.DisplayHorizontal = Style.FontColor
        }

        protected virtual void Uninitialize()
        {
            this.State &= ~ElementState.Initialized;

            this.Selector.Parent = null;
            _styles = null!;

            this.container = null!;
        }

        protected virtual void Draw(GameTime gameTime)
        {
            var result = this.Styles.TryGet<Color>(Style.FontColor, ElementState.Initialized, out var color);
        }

        protected virtual void Update(GameTime gameTime)
        {

        }

        protected virtual void Clean()
        {
            this.CleanOuterBounds(ref _outerBounds);
        }

        protected virtual void CleanOuterBounds(ref Rectangle bounds)
        {

        }

        protected virtual void Initialize(IContainer container, out IStyleProvider styles, out Selector? parent)
        {
            styles = container.Styles.Source.GetProvider(this.Selector);
            parent = container.Selector;
        }

        void IElement.Initialize(IContainer container)
        {
            this.Initialize(container);
        }

        void IElement.Uninitialize()
        {
            this.Uninitialize();
        }

        void IElement.Draw(GameTime gameTime)
        {
            this.Draw(gameTime);
        }

        void IElement.Update(GameTime gameTime)
        {
            this.Update(gameTime);
        }

        void IElement.Clean()
        {
            this.Clean();
        }
    }
}
