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
    [AutoLoad]
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
            _inputs = inputs;
            _buttons = buttons.ToDictionary(x => x.Key, x => x);
            _providers = providers.ToArray();

            foreach (var provider in _providers)
            {
                provider.Clean(_buttons.Values);
            }
        }

        [SequenceGroup<InitializeComponentSequenceGroup>(InitializeComponentSequenceGroup.Initialize)]
        public void Initialize(IGuppyEngine engine)
        {
            //
        }

        [SequenceGroup<UpdateComponentSequenceGroup>(UpdateComponentSequenceGroup.PreUpdate)]
        public void Update(GameTime gameTime)
        {
            foreach (var provider in _providers)
            {
                foreach (IInput data in provider.Update())
                {
                    _inputs.Publish(data);
                }
            }
        }
    }
}
