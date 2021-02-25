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
using Guppy.UI.Interfaces;
using Guppy.UI.Layers;
using Guppy.UI.Lists;
using Guppy.UI.Services;
using Guppy.Utilities.Cameras;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ServiceCollection = Guppy.DependencyInjection.ServiceCollection;
using ServiceProvider = Guppy.DependencyInjection.ServiceProvider;

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
            services.AddTransient<ScreenLayer>();
            services.AddTransient<Camera2D>("screen-camera");

            // Register default elements
            services.AddFactory(typeof(ElementList<>), (p, t) => ActivatorUtilities.CreateInstance(p, t));
            services.AddFactory(typeof(Container<>), (p, t) => ActivatorUtilities.CreateInstance(p, t));
            services.AddFactory<Container>(p => new Container());
            services.AddFactory<PageContainer>(p => new PageContainer());
            services.AddFactory(typeof(StackContainer<>), (p, t) => ActivatorUtilities.CreateInstance(p, t));
            services.AddFactory(typeof(InnerStackContainer<>), (p, t) => ActivatorUtilities.CreateInstance(p, t));
            services.AddFactory<StackContainer>(p => new StackContainer());
            services.AddFactory<Element>(p => new Element());
            services.AddFactory<TextElement>(p => new TextElement());
            services.AddFactory<TextInput>(p => new TextInput());

            services.AddTransient(typeof(ElementList<>));
            services.AddTransient(typeof(Container<>));
            services.AddTransient<Container>();
            services.AddTransient<PageContainer>();
            services.AddTransient(typeof(StackContainer<>));
            services.AddTransient(typeof(InnerStackContainer<>));
            services.AddTransient<StackContainer>();
            services.AddTransient<Element>();
            services.AddTransient<TextElement>();
            services.AddTransient<TextInput>();

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
                Lockable = true,
                Commands = new[]
                {
                    (state: ButtonState.Pressed, command: "ui interact -s=pressed"),
                    (state: ButtonState.Released, command: "ui interact -s=released")
                }
            });
        }
    }
}
