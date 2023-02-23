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
        public IStyleProvider Styles { get; }
        public ElementState State { get; }

        public Element()
        {
            this.Styles = new StyleProvider(this);
        }

        protected virtual void PreDraw(GameTime gameTime)
        {
        }

        protected void Draw(GameTime gameTime)
        {

        }

        protected virtual void PostDraw(GameTime gameTime)
        {
        }

        protected virtual void Clean(IStyleStack styles)
        {
            styles.Push(this.Styles);



            styles.Pop();
        }

        void IElement.Draw(GameTime gameTime)
        {
            this.PreDraw(gameTime);
            this.Draw(gameTime);
            this.PostDraw(gameTime);
        }

        void IElement.Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        void IElement.Clean(IStyleStack styles)
        {
            this.Clean(styles);
        }
    }
}
