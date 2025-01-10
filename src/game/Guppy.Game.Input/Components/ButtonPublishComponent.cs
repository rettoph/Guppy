using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Enums;
using Guppy.Game.Input.Common;
using Guppy.Game.Input.Common.Services;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Input.Components
{
    internal sealed class ButtonPublishComponent : IEngineComponent, IUpdatableComponent
    {
        private readonly IInputService _inputs;
        private readonly Dictionary<string, IButton> _buttons;
        private readonly IButtonService[] _providers;

        public ButtonPublishComponent(
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

        [SequenceGroup<InitializeComponentSequenceGroupEnum>(InitializeComponentSequenceGroupEnum.Initialize)]
        public void Initialize(IGuppyEngine engine)
        {
            //
        }

        [SequenceGroup<UpdateComponentSequenceGroupEnum>(UpdateComponentSequenceGroupEnum.PreUpdate)]
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