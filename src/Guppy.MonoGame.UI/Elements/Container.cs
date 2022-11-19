using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Elements
{
    public abstract class Container : Element
    {
        public readonly IList<Element> Children;

        public string Name;

        public Container(string name) : base()
        {
            this.Name = name;
            this.Children = new List<Element>();
        }

        protected override void InnerDraw(GameTime gameTime)
        {
            if (this.BeginDrawContainer())
            {
                this.DrawChildren(gameTime);
            }

            this.EndDrawContainer();
        }

        protected virtual void DrawChildren(GameTime gameTime)
        {
            foreach (Element child in this.Children)
            {
                child.Draw(gameTime);
            }
        }

        protected abstract bool BeginDrawContainer();
        protected abstract void EndDrawContainer();
    }

    public abstract class Container<TFlags> : Container
        where TFlags : struct, Enum
    {
        public TFlags Flags;

        public Container(string name) : base(name)
        {
        }
    }
}
