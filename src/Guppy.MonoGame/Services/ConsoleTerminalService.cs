using Guppy.Attributes;
using Guppy.Commands.Services;
using Guppy.MonoGame.Helpers;
using Microsoft.Xna.Framework;

namespace Guppy.MonoGame.Services
{
    internal sealed class ConsoleTerminalService : ITerminalService
    {
        private readonly ICommandService _commands;
        private readonly CancellationTokenSource _token;

        public ConsoleTerminalService(ICommandService commands)
        {
            _commands = commands;

            _token = new CancellationTokenSource();

            Task.Run(this.Loop, _token.Token);
        }

        private void Loop()
        {
            while(!_token.IsCancellationRequested)
            {
                var input = Console.ReadLine();

                if (input is null)
                {
                    return;
                }

                if(_token.IsCancellationRequested)
                {
                    return;
                }

                _commands.Invoke(input);
            }

        }

        public void WriteLine(string text, Color color)
        {
            Console.WriteLine(text);
        }
    }
}
