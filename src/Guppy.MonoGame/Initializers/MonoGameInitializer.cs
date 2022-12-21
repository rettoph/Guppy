using Guppy.Common.Providers;
using Guppy.Initializers;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Initializers
{
    internal sealed class MonoGameInitializer : IGuppyInitializer
    {
        public void Initialize(IAssemblyProvider assemblies, IServiceCollection services, IEnumerable<IGuppyLoader> loaders)
        {
            assemblies.Load(typeof(Game).Assembly, true);

            var vertexTypes = assemblies.GetTypes<IVertexType>().Where(t => t.IsValueType);

            foreach(Type vertexType in vertexTypes)
            {
                var primitiveBatchType = typeof(PrimitiveBatch<>).MakeGenericType(vertexType);
                services.AddSingleton(primitiveBatchType);
            }
        }
    }
}
