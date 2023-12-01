using Autofac;
using Guppy.Common;
using Guppy.Common.Extensions;
using Guppy.Extensions.Autofac;
using Guppy.MonoGame.Loaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterMonoGame(
            this ContainerBuilder builder, 
            Game game,
            GraphicsDeviceManager graphics, 
            ContentManager content, 
            GameWindow window)
        {
            if(builder.HasTag(nameof(RegisterMonoGame)))
            {
                return builder;
            }

            builder.RegisterInstance(game).SingleInstance();
            builder.RegisterInstance<GraphicsDeviceManager>(graphics).SingleInstance();
            builder.RegisterInstance<GraphicsDevice>(graphics.GraphicsDevice).SingleInstance();
            builder.RegisterInstance<ContentManager>(content).SingleInstance();
            builder.RegisterInstance<GameWindow>(window).SingleInstance();

            return builder.RegisterServiceLoader<MonoGameLoader>().AddTag(nameof(RegisterMonoGame));
        }
    }
}
