using Autofac;
using Guppy.Core.Commands.Extensions;
using Guppy.Core.Common.Extensions.Autofac;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Core.Files
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterMonoGame(
            this ContainerBuilder builder,
            Microsoft.Xna.Framework.Game game,
            GraphicsDeviceManager graphics,
            ContentManager content,
            GameWindow window)
        {
            if (builder.HasTag(nameof(RegisterMonoGame)))
            {
                return builder;
            }

            builder.RegisterCoreCommandServices();

            builder.RegisterInstance(game).SingleInstance();
            builder.RegisterInstance<GraphicsDeviceManager>(graphics).SingleInstance();
            builder.RegisterInstance<GraphicsDevice>(graphics.GraphicsDevice).SingleInstance();
            builder.RegisterInstance<ContentManager>(content).SingleInstance();
            builder.RegisterInstance<GameWindow>(window).SingleInstance();

            return builder.AddTag(nameof(RegisterMonoGame));
            // return builder.RegisterServiceLoader<MonoGameLoader>().AddTag(nameof(RegisterMonoGame));
        }
    }
}
