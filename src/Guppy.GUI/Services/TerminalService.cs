using Guppy.Commands.Services;
using Guppy.Common;
using Guppy.GUI.Constants;
using Guppy.GUI.Elements;
using Guppy.GUI.Messages;
using Guppy.GUI.Providers;
using Guppy.MonoGame;
using Microsoft.Xna.Framework;

namespace Guppy.GUI.Services
{
    internal sealed partial class TerminalService : SimpleDrawableGameComponent, 
        ISubscriber<ToggleTerminal>
    {
        private readonly Stage _stage;
        private readonly Output _output;
        private readonly TextInput _input;
        private readonly ICommandService _commands;

        public TerminalService(
            IStageProvider stages,
            ICommandService commands)
        {
            _stage = stages.Create(ElementNames.Terminal);
            _commands = commands;
            _output = new Output(_stage);
            _input = new TextInput(ElementNames.TerminalInput)
            {
                Blacklist =
                {
                    '`',
                    '~'
                }
            };

            _stage.Add(_output);
            _stage.Add(_input);

            _input.OnEntered += this.HandleInputEntered;

            this.Visible = false;
            this.IsEnabled = false;
        }

        private void HandleInputEntered(TextInput sender, string args)
        {
            _commands.Invoke(_input.Value);
            _input.Value = string.Empty;
        }

        public override void Draw(GameTime gameTime)
        {
            _input.State |= ElementState.Focused;
            _stage.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public void Process(in ToggleTerminal message)
        {
            this.Visible = !this.Visible;
        }
    }
}
