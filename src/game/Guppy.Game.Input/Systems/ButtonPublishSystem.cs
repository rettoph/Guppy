using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Systems;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common.Systems;
using Guppy.Game.Input.Common;
using Guppy.Game.Input.Common.Services;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Input.Systems
{
    public sealed class ButtonPublishSystem : IEngineSystem, IUpdateSystem
    {
        private readonly IInputService _inputs;
        private readonly Dictionary<string, IButton> _buttons;
        private readonly IButtonService[] _providers;

        public ButtonPublishSystem(
            IInputService inputs,
            IEnumerable<IButton> buttons,
            IEnumerable<IButtonService> providers)
        {
            this._inputs = inputs;
            this._buttons = buttons.ToDictionary(x => x.Key, x => x);
            this._providers = providers.ToArray();

            foreach (var provider in this._providers)
            {
                provider.Clean(this._buttons.Values);
            }
        }

        [SequenceGroup<UpdateSequenceGroupEnum>(UpdateSequenceGroupEnum.PreUpdate)]
        public void Update(GameTime gameTime)
        {
            foreach (var provider in this._providers)
            {
                foreach (IInput data in provider.Update())
                {
                    this._inputs.Publish(data);
                }
            }
        }
    }
}