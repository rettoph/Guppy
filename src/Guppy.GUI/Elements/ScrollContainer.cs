using Guppy.Common;
using Guppy.Input.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Elements
{
    public class ScrollContainer<T> : Container<T>, ISubscriber<CursorScroll>
        where T : Element
    {
        public ScrollContainer(params string[] names) : base(names)
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

        public void Process(in CursorScroll message)
        {
            if(!this.state.HasFlag(ElementState.Hovered))
            {
                return;
            }

            this.contentOffset.Y += message.Delta;

            this.contentOffset.Y = Math.Clamp(this.contentOffset.Y, this.InnerBounds.Height - this.ContentBounds.Height, 0);
        }
    }
}
