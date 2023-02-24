using Guppy.MonoGame.UI.Providers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Elements
{
    public class Container<TChildren> : Element, IContainer<TChildren>
        where TChildren : IElement
    {
        private IList<TChildren> _children;

        public readonly ReadOnlyCollection<TChildren> Children;

        ReadOnlyCollection<TChildren> IContainer<TChildren>.Children => this.Children;

        IEnumerable<IElement> IContainer.Children => _children.Select(x => (IElement)x);

        public Container()
        {
            _children = new List<TChildren>();

            this.Children = new ReadOnlyCollection<TChildren>(_children);
        }

        protected virtual void Add(TChildren child)
        {
            _children.Add(child);
            child.Initialize(this);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            foreach (var child in this.Children)
            {
                child.Draw(gameTime);
            }
        }

        protected virtual bool Remove(TChildren child)
        {
            if(_children.Remove(child))
            {
                return true;
            }

            return false;
        }
    }
}
