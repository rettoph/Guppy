using Guppy.Attributes;
using Guppy.Common;
using Guppy.MonoGame.Messages.Inputs;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Guppy.MonoGame.Services
{
    [AutoSubscribe]
    internal abstract class BaseWindowService : SimpleDrawableGameComponent, ISubscriber<ToggleWindowInput>
    {
        public abstract ToggleWindowInput.Windows Window { get; }

        public BaseWindowService(bool visible)
        {
            this.Visible = visible;
        }

        public void Process(in ToggleWindowInput message)
        {
            if(message.Window == this.Window)
            {
                this.Visible = !this.Visible;
            }
        }
    }
}
