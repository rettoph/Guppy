using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Timers;
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
            this.Clean();
        }

        protected virtual void Remove(T child)
        {
            if(!_children.Remove(child))
            {
                return;
            }

            child.Uninitialize();
            this.Clean();
        }

        protected override void DrawContent(GameTime gameTime, Vector2 position)
        {
            base.DrawContent(gameTime, position);

            foreach (T child in _children)
            {
                child.Draw(gameTime, position);
            }
        }

        protected internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (T child in _children)
            {
                child.Update(gameTime);
            }
        }

        protected override void CleanContentBounds(in RectangleF constraints, out RectangleF contentBounds)
        {
            base.CleanContentBounds(constraints, out contentBounds);
            Vector2 size = Vector2.Zero;

            foreach (T child in _children)
            {
                child.Clean();
                size.X += child.OuterBounds.Width;
                size.Y += child.OuterBounds.Height;
            }

            contentBounds.Width = size.X;
            contentBounds.Height = size.Y;
        }
    }
}
