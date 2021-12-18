using Guppy.Interfaces;
using Guppy.Services;
using Guppy.Utilities;
using Guppy.Utilities.Cameras;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.DependencyInjection.Builders;

namespace Guppy.ServiceLoaders
{
    /// <summary>
    /// Non autoloaded ServiceLoader, this is added by the
    /// <see cref="GuppyLoader.ConfigureMonoGame"/> extension
    /// method.
    /// </summary>
    internal class MonoGameServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, ServiceProviderBuilder services)
        {
            services.RegisterTypeFactory<SpriteBatch>()
                .SetMethod(p => new SpriteBatch(p.GetService<GraphicsDevice>()));

            services.RegisterTypeFactory<Camera2D>()
                .SetDefaultConstructor<Camera2D>();

            services.RegisterService<SpriteBatch>()
                .SetLifetime(ServiceLifetime.Scoped)
                .SetFactoryType<SpriteBatch>();

            services.RegisterService<SpriteBatch>(Guppy.Constants.ServiceNames.TransientSpritebatch)
                .SetLifetime(ServiceLifetime.Transient)
                .SetFactoryType<SpriteBatch>();

            services.RegisterService<Camera2D>()
                .SetLifetime(ServiceLifetime.Scoped)
                .SetFactoryType<Camera2D>();

            services.RegisterService<Camera2D>(Guppy.Constants.ServiceNames.TransientCamera)
                .SetLifetime(ServiceLifetime.Transient)
                .SetFactoryType<Camera2D>();

            assemblyHelper.AddAssembly(typeof(GraphicsDevice).Assembly);

            assemblyHelper.Types.GetTypesAssignableFrom<IVertexType>().Where(t => t.IsValueType).ForEach(vt =>
            {
                var primitiveBatchType = typeof(PrimitiveBatch<>).MakeGenericType(vt);
                services.RegisterService(primitiveBatchType.FullName)
                    .SetLifetime(ServiceLifetime.Singleton)
                    .SetTypeFactory(primitiveBatchType, factory =>
                    {
                        factory.SetMethod(p => Activator.CreateInstance(primitiveBatchType, p.GetService<GraphicsDevice>()));
                    });
            });
        }
    }
}
