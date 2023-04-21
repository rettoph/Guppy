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
        public IMessage? PressedMessage { get; set; }
        public IMessage? ReleasedMessage { get; set; }

        public event OnEventDelegate<Button<T>, IMessage?>? OnPressed;
        public event OnEventDelegate<Button<T>, IMessage?>? OnReleased;

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

            if(!wasActive && wasActive)
            {
                if (this.PressedMessage is not null)
                {
                    this.stage.Bus.Enqueue(this.PressedMessage);
                }

                this.OnPressed?.Invoke(this, this.PressedMessage);
                
                return;
            }

            if(wasActive && !isActive && isHovered)
            {
                if (this.ReleasedMessage is not null)
                {
                    this.stage.Bus.Enqueue(this.ReleasedMessage);
                }

                this.OnReleased?.Invoke(this, this.ReleasedMessage);

                return;
            }
        }
    }
}
