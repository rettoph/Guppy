using Guppy.Attributes;
using Guppy.Interfaces;
using Guppy.Utilities.Loaders;
using Guppy.Utilities.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Pong.Client.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.Client.ServiceLoaders
{
    [IsServiceLoader]
    public class PongServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<GameOptions>(go =>
            {
                go.LogLevel = LogLevel.Trace;
            });
        }

        public void ConfigureProvider(IServiceProvider provider)
        {
            var strings = provider.GetService<StringLoader>();
            strings.TryRegister("name:entity:paddle:human", "Human Player");
            strings.TryRegister("name:entity:paddle:ai", "AI Player");
            strings.TryRegister("description:entity:paddle", "A player.");

            var entities = provider.GetService<EntityLoader>();

            entities.TryRegister<FieldEntity>("pong:field");
            entities.TryRegister<BallEntity>("pong:ball");
            entities.TryRegister<HumanPaddleEntity>("pong:paddle:human", "name:entity:paddle:human", "description:entity:paddle", Matrix.CreateRotationZ(0));
            entities.TryRegister<PaddleEntity>("pong:paddle:ai", "name:entity:paddle:ai", "description:entity:paddle", Matrix.CreateRotationZ(MathHelper.Pi));
        }
    }
}
