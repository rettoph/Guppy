using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Extensions.log4net;
using Guppy.Interfaces;
using Guppy.IO.Commands.Contexts;
using Guppy.IO.Commands.Services;
using Guppy.IO.Input;
using Guppy.IO.Input.Contexts;
using Guppy.IO.Input.Services;
using Guppy.IO.Output.Drivers.Scenes;
using Guppy.IO.Output.Services;
using Guppy.Services;
using log4net;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Output.ServiceLoaders
{
    internal sealed class TerminalServiceLoader : IServiceLoader
    {
        #region Private Fields
        private String _defaultTerminalFontPath;
        #endregion

        #region Constructor
        internal TerminalServiceLoader(String defaultTerminalFontPath)
        {
            _defaultTerminalFontPath = defaultTerminalFontPath;
        }
        #endregion

        #region IServiceLoader Implementation
        public void RegisterServices(ServiceCollection services)
        {
            services.AddFactory<TerminalService>(p => new TerminalService());
            services.AddSingleton<TerminalService>();

            services.AddSetup<ContentService>((content, p, c) =>
            {
                content.TryRegister("font:terminal", _defaultTerminalFontPath);
            });

            services.AddAndBindDriver<Game, GameTerminalDriver>(p => new GameTerminalDriver());

            services.AddSetup<ILog>((logger, p, c) =>
            {
                logger.AddAppender(p.GetService<TerminalService>());
            });
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            var commands = provider.GetService<CommandService>();
            var inputs = provider.GetService<InputCommandService>();

            commands.TryAddCommand(new CommandContext()
            {
                Word = "terminal"
            });

            inputs.Add(new InputCommandContext()
            {
                Handle = "guppy_terminal",
                DefaultInput = new InputType(Keys.OemTilde),
                Commands = new[]
                {
                    (state: ButtonState.Pressed, command: "terminal")
                }
            });
        }
        #endregion
    }
}
