using Autofac;
using Guppy.Common.Autofac;
using Guppy.Game.ImGui;
using Guppy.Game.MonoGame.Components.Guppy;
using Guppy.Game.MonoGame.Constants;
using Guppy.Game.MonoGame.Messages;
using Guppy.Game.MonoGame.Primitives;
using Guppy.Loaders;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Guppy.Game.MonoGame.Loaders
{
    internal sealed class MonoGameLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<Screen>().As<IScreen>().InstancePerLifetimeScope();
            services.RegisterType<SpriteBatch>().SingleInstance();

            services.RegisterType<MonoGameTerminal>().AsImplementedInterfaces().AsSelf().InstancePerMatchingLifetimeScope(LifetimeScopeTags.GuppyScope);

            services.RegisterType<MonoGameImGuiBatch>().AsImplementedInterfaces().SingleInstance();

            services.RegisterGeneric(typeof(PrimitiveBatch<,>));
            services.RegisterGeneric(typeof(PrimitiveBatch<>));

            services.RegisterInput(Inputs.ToggleDebugger, Keys.F1, new[]
            {
                (ButtonState.Pressed, Toggle<GuppyDebugWindowComponent>.Instance)
            });

            services.RegisterInput(Inputs.ToggleTerminal, Keys.OemTilde, new[]
{
                (ButtonState.Pressed, Toggle<TerminalWindowComponent>.Instance)
            });
        }
    }
}
