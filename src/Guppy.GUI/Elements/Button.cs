using Guppy.Common;
using Guppy.GUI.Constants;
using Guppy.MonoGame.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Elements
{
    public class Button<T> : Input<T>
        where T : Element
    {
        public IMessage? OnPressed { get; set; }
        public IMessage? OnReleased { get; set; }

        public Button(params string[] names) : this((IEnumerable<string>)names)
        {
        }
        public Button(IEnumerable<string> names) : base(names)
        {
            this.OnStateChanged += this.HandleStateChanged;
        }

        private void HandleStateChanged(Element sender, ElementState old, ElementState value)
        {
            bool wasActive = old.HasFlag(ElementState.Active);
            bool isActive = value.HasFlag(ElementState.Active);
            bool isHovered = value.HasFlag(ElementState.Hovered);

            if(!wasActive && wasActive && this.OnPressed is not null)
            {
                this.stage.Bus.Enqueue(this.OnPressed);
                return;
            }

            if(wasActive && !isActive && isHovered && this.OnReleased is not null)
            {
                this.stage.Bus.Enqueue(this.OnReleased);
                return;
            }
        }
    }
}
