using Guppy.Common.DependencyInjection;
using Guppy.GUI.Constants;
using Guppy.GUI.Messages;
using Guppy.GUI.Providers;
using Guppy.GUI.Services;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Loaders
{
    internal sealed class GUIServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddService<IStageProvider>()
                .SetLifetime(ServiceLifetime.Scoped)
                .SetImplementationType<StageProvider>();

            services.ConfigureCollection(manager =>
            {
                manager.AddScoped<IScreen>().SetImplementationType<Screen>();

                manager.AddScoped<TerminalService>().AddInterfaceAliases();
            });

            services.AddInput(Inputs.ToggleTerminal, Keys.OemTilde, new[]
            {
                (false, new ToggleTerminal())
            });
        }
    }
}
