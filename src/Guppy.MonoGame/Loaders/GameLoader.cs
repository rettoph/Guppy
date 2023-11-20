using Autofac;
using Guppy.Common;
using Guppy.Common.Extensions.Autofac;
using Guppy.Common.Filters;
using Guppy.Loaders;
using Guppy.MonoGame.Constants;
using Guppy.MonoGame.Providers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Loaders
{
    internal sealed class GameLoader : IServiceLoader
    {
        public void ConfigureServices(ContainerBuilder services)
        {
            services.RegisterType<MenuProvider>().As<IMenuProvider>().InstancePerLifetimeScope();

            services.AddFilter(new ServiceFilter<IGameComponent, Type>(typeof(MonoGameGuppy)));
        }
    }
}
