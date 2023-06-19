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
        public Input(IEnumerable<string> names) : base(names)
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

        public virtual void Process(in Guid messageId, in CursorPress message)
        {
            if(message.Button != CursorButtons.Left)
            {
                return;
            }

            bool hovered = this.State.HasFlag(ElementState.Hovered);
            bool pressed = message.Value;
            bool focused = this.State.HasFlag(ElementState.Active);

            if (hovered)
            {
                if (pressed)
                {
                    this.State |= ElementState.Active;
                }
                else if(focused)
                {
                    this.State |= ElementState.Focused;
                    this.State &= ~ElementState.Active;
                }
            }
            else
            {
                this.State &= ~ElementState.Active;

                if (pressed)
                {
                    this.State &= ~ElementState.Focused;
                }
            }
        }
    }
}
