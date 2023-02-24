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
        public Selector Selector { get; }
        public IStyleProvider Styles { get; private set; }
        public ElementState State { get; protected set; }

        public Rectangle OuterBounds { get; set; }
        public Rectangle InnerBounds { get; set; }

        public Element(params string[] names)
        {
            this.Selector = new Selector(this.GetType(), names);
            this.Styles = null!;
        }

        protected virtual void Initialize(IContainer container)
        {
            this.Selector.Parent = this.InitializeSelectorParent(container);
            this.Styles = this.InitializeStyleProvider(container);
            this.State |= ElementState.Initialized;
        }

        protected virtual void Uninitialize()
        {
            this.State &= ~ElementState.Initialized;
            this.Styles = null!;
            this.Selector.Parent = null;
        }

        protected virtual void Draw(GameTime gameTime)
        {
            var result = this.Styles.TryGet<Color>(Style.Color, ElementState.Initialized, out var color);
        }

        protected virtual void Update(GameTime gameTime)
        {

        }

        protected virtual void Clean()
        {

        }

        protected virtual IStyleProvider InitializeStyleProvider(IContainer container)
        {
            return container.Styles.Source.GetProvider(this.Selector);
        }

        protected virtual Selector? InitializeSelectorParent(IContainer container)
        {
            return container.Selector;
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
