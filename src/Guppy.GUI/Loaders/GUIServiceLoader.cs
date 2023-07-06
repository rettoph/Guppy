using Autofac;
using Guppy.GUI.Constants;
using Guppy.GUI.Messages;
using Guppy.GUI.Providers;
using Guppy.Loaders;
using Microsoft.Xna.Framework.Input;

namespace Guppy.GUI.Loaders
{
    internal sealed class GUIServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<StageProvider>().As<IStageProvider>().InstancePerLifetimeScope();
            services.RegisterType<Screen>().As<IScreen>().InstancePerLifetimeScope();

            services.AddInput(Inputs.ToggleTerminal, Keys.OemTilde, new[]
            {
                (false, new ToggleTerminal())
            });
        }
    }
}
