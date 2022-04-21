using Guppy.Gaming.Components;
using Guppy.Gaming.Constants;
using Guppy.Gaming.Graphics;
using Guppy.Gaming.Services;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaColor = Microsoft.Xna.Framework.Color;

namespace Guppy.Gaming.Loaders
{
    internal sealed class MonoGameServiceLoader : IServiceLoader
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly ContentManager _content;
        private readonly GameWindow _window;

        public MonoGameServiceLoader(GraphicsDeviceManager graphics, ContentManager content, GameWindow window)
        {
            _graphics = graphics;
            _content = content;
            _window = window;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<GraphicsDeviceManager>(_graphics);
            services.AddSingleton<GraphicsDevice>(_graphics.GraphicsDevice);
            services.AddSingleton<ContentManager>(_content);
            services.AddSingleton<GameWindow>(_window);
            services.AddTransient<BasicEffect>();
            services.AddTransient<SpriteBatch>();
            services.AddTransient(typeof(PrimitiveBatch<,>));
            services.AddTransient(typeof(PrimitiveBatch<>));
            services.AddTransient<Camera2D>();

            services.AddSingleton<ITerminalService, TerminalService>();
            services.AddComponent<Game, GameMonoGameServiceComponent>();

            services.AddSetting<int>(SettingConstants.TerminalBufferLength, 256, true, SettingConstants.GamingTag);

            services.AddColor(ColorConstants.TerminalBackgroundColor, XnaColor.Lerp(XnaColor.Transparent, XnaColor.Black, 0.5f));
            services.AddColor(ColorConstants.TerminalBackgroundTextColor, XnaColor.White);
            services.AddColor(ColorConstants.TerminalInputBackgroundColor, XnaColor.Lerp(XnaColor.Transparent, XnaColor.LightGray, 0.75f));
            services.AddColor(ColorConstants.TerminalInputTextColor, XnaColor.Black);

            services.AddContent<SpriteFont>(ContentConstants.DiagnosticsFont, "Fonts/DiagnosticsFont");
        }
    }
}
