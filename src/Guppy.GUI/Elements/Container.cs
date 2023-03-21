using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Elements
{
    public class Container<T> : Element
        where T : Element
    {
        private readonly IList<T> _children;

        protected readonly ReadOnlyCollection<T> children;

        public Container(params string[] names) : base(names)
        {
            _children = new List<T>();

            this.children = new ReadOnlyCollection<T>(_children);
        }

        protected virtual void Add(T child)
        {
            _children.Add(child);

            child.Initialize(this.stage, this);
            this.Clean(out _);
        }

        protected virtual void Remove(T child)
        {
            if(!_children.Remove(child))
            {
                return;
            }

            child.Uninitialize();
            this.Clean(out _);
        }

        protected override void InnerDraw(GameTime gameTime, Point position)
        {
            base.InnerDraw(gameTime, position);

            foreach (T child in _children)
            {
                child.Draw(gameTime, position);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (T child in _children)
            {
                child.Update(gameTime);
            }
        }

        protected override Point CleanContentSize(Point constraints)
        {
            Point size = Point.Zero;
            Point translation = Point.Zero;

            foreach(T child in _children)
            {
                child.Clean(out var childSize);

                translation.Y += childSize.Y;
                size.Y += childSize.Y;
                size.X = Math.Max(childSize.X, size.X);
            }

            return size;
        }
    }
}
