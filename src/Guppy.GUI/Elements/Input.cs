using Guppy.Common;
using Guppy.Input.Enums;
using Guppy.Input.Messages;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Elements
{
    public abstract class Input<T> : Container<T>, ISubscriber<CursorPress>
        where T : Element
    {
        public Input(params string[] names) : base(names)
        {

        }

        protected internal override void Initialize(Stage stage, Element? parent)
        {
            base.Initialize(stage, parent);

            this.stage.Bus.Subscribe(this);
        }

        protected internal override void Uninitialize()
        {
            base.Uninitialize();

            this.stage.Bus.Unsubscribe(this);
        }

        public virtual void Process(in CursorPress message)
        {
            if(message.Button != CursorButtons.Left)
            {
                return;
            }

            bool hovered = this.State.HasFlag(ElementState.Hovered);
            bool pressed = message.Value;
            bool focused = this.State.HasFlag(ElementState.Focused);

            if (hovered)
            {
                if (pressed)
                {
                    this.State |= ElementState.Focused;
                }
                else if(focused)
                {
                    this.State |= ElementState.Focus;
                    this.State &= ~ElementState.Focused;
                }
            }
            else
            {
                this.State &= ~ElementState.Focused;

                if (pressed)
                {
                    this.State &= ~ElementState.Focus;
                }
            }
        }
    }
}
