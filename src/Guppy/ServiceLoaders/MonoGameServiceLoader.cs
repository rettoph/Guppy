using Guppy.DependencyInjection;
using Guppy.Extensions.System.Collections;
using Guppy.Extensions.System;
using Guppy.Interfaces;
using Guppy.Services;
using Guppy.Utilities;
using Guppy.Utilities.Cameras;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.ServiceLoaders
{
    /// <summary>
    /// Non autoloaded ServiceLoader, this is added by the
    /// <see cref="GuppyLoader.ConfigureMonoGame"/> extension
    /// method.
    /// </summary>
    internal class MonoGameServiceLoader : IServiceLoader
    {
        public void RegisterServices(ServiceCollection services)
        {
            services.AddFactory<SpriteBatch>(p => new SpriteBatch(p.GetService<GraphicsDevice>()));
            services.AddFactory<Camera2D>(p => new Camera2D());

            services.AddScoped<SpriteBatch>();
            services.AddScoped<Camera2D>();

            AssemblyHelper.AddAssembly(typeof(GraphicsDevice).Assembly);
            AssemblyHelper.Types.GetTypesAssignableFrom<IVertexType>().Where(t => t.IsValueType).ForEach(vt =>
            {
                var primitiveBatchType = typeof(PrimitiveBatch<>).MakeGenericType(vt);
                services.AddFactory(primitiveBatchType, (p, t) => Microsoft.Extensions.DependencyInjection.ActivatorUtilities.CreateInstance(p, t));
                services.AddSingleton(primitiveBatchType);
            });
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
