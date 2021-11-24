using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Services;
using Guppy.Utilities;
using Guppy.Utilities.Cameras;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.ServiceLoaders
{
    /// <summary>
    /// Non autoloaded ServiceLoader, this is added by the
    /// <see cref="GuppyLoader.ConfigureMonoGame"/> extension
    /// method.
    /// </summary>
    internal class MonoGameServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceCollection services)
        {
            services.RegisterTypeFactory<SpriteBatch>(p => new SpriteBatch(p.GetService<GraphicsDevice>()));
            services.RegisterTypeFactory<Camera2D>(p => new Camera2D());

            services.RegisterScoped<SpriteBatch>();
            services.RegisterTransient(Guppy.Constants.ServiceConfigurationKeys.TransientSpritebatch);
            services.RegisterScoped<Camera2D>();
            services.RegisterTransient(Guppy.Constants.ServiceConfigurationKeys.TransientCamera);

            assemblyHelper.AddAssembly(typeof(GraphicsDevice).Assembly);

            assemblyHelper.Types.GetTypesAssignableFrom<IVertexType>().Where(t => t.IsValueType).ForEach(vt =>
            {
                var primitiveBatchType = typeof(PrimitiveBatch<>).MakeGenericType(vt);
                services.RegisterTypeFactory(primitiveBatchType, (p, t) => Activator.CreateInstance(t, p.GetService<GraphicsDevice>()));
                services.RegisterSingleton(ServiceConfigurationKey.From(type: primitiveBatchType));
            });
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
