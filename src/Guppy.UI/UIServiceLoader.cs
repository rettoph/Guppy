using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.IO.Commands;
using Guppy.IO.Commands.Contexts;
using Guppy.IO.Commands.Services;
using Guppy.IO.Enums;
using Guppy.IO.Input;
using Guppy.IO.Input.Contexts;
using Guppy.IO.Input.Services;
using Guppy.Lists;
using Guppy.Services;
using Guppy.UI.Elements;
using Guppy.UI.Entities;
using Guppy.UI.Enums;
using Guppy.UI.Extensions.DependencyInjection;
using Guppy.UI.Interfaces;
using Guppy.UI.Layers;
using Guppy.UI.Services;
using Guppy.Utilities.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Guppy.UI
{
    [AutoLoad]
    internal sealed class UIServiceLoader : IServiceLoader
    {
        public void RegisterServices(ServiceCollection services)
        {
            services.AddFactory<UIService>(p => new UIService());
            services.AddFactory<Stage>(p => new Stage());
            services.AddFactory<ScreenLayer>(p => new ScreenLayer());

            services.AddSingleton<UIService>();
            services.AddTransient<Stage>();
            services.AddScoped<ScreenLayer>();
            services.AddTransient<Camera2D>("screen-camera");

            services.AddUIComponent<Container<IElement>>(p => new Container<IElement>());
            services.AddUIComponent<Element>(p => new Element());
            services.AddUIComponent<TextElement>(p => new TextElement());

            // Register Content
            services.AddSetup<ContentService>((content, p, c) =>
            {
                content.TryRegister("ui:font", "UI/Font");
            });

            services.AddSetup<TextElement>((text, p, c) =>
            {
                text.Font = p.GetContent<SpriteFont>("ui:font");
                text.Alignment = Alignment.CenterLeft;
            }, -1);
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            var commands = provider.GetService<CommandService>();
            var inputs = provider.GetService<InputCommandService>();

            commands.TryAddCommand(new CommandContext()
            {
                Word = "ui",
                Commands = new CommandContext[] {
                    new CommandContext()
                    {
                        Word = "interact",
                        Arguments = new ArgContext[]
                        {
                            new ArgContext()
                            {
                                Identifier = "state",
                                Aliases = "s".ToCharArray(),
                                Required = true,
                                Type = ArgType.FromEnum<ButtonState>()
                            }
                        }
                    }
                }
            });

            inputs.Add(new InputCommandContext()
            {
                Handle = "guppy_ui_interact",
                DefaultInput = new InputType(MouseButton.Left),
                Commands = new[]
                {
                    (state: ButtonState.Pressed, command: "ui interact -s=pressed"),
                    (state: ButtonState.Released, command: "ui interact -s=released")
                }
            });
        }
    }
}
