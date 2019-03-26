using Guppy.Network.Groups;
using Guppy.Network.Peers;
using Guppy.UI.Entities;
using Guppy.UI.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Client.Layers;
using Pong.Library.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Utilities.Units;
using Guppy.UI.Elements;
using Guppy.UI.StyleSheets;
using Guppy.UI.Enums;
using Guppy.Extensions;
using Guppy.Loaders;

namespace Pong.Client.Scenes
{
    public class ClientLobbyScene : LobbyScene
    {
        private GraphicsDevice _graphics;
        private SpriteFont _font;

        public ClientLobbyScene(GraphicsDevice graphics, Peer peer, IServiceProvider provider) : base(peer, provider)
        {
            _graphics = graphics;
            _font = provider.GetLoader<ContentLoader>().Get<SpriteFont>("ui:font");
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.layers.Create<SimpleLayer>();
            var styleSheet = new StyleSheet();
            styleSheet.SetProperty(StyleProperty.Font, _font);
            styleSheet.SetProperty(StyleProperty.FontColor, Color.White);

            var stage = this.entities.Create("ui:stage") as Stage;
            var sidebar = stage.Content.Add(new Container(0, 0, 200, 1f)) as Container;
            var content = stage.Content.Add(new Container(200, 0, new Unit[] { 1f, -200 }, 1f)) as Container;

            content.Add(new TextElement(0.25f, 0.25f, 0.5f, 0.5f, "Hello World", styleSheet));
            content.Add(new TextElement(0.1f, 0.1f, 0.1f, 0.1f, "A", styleSheet));
            content.Add(new TextElement(0.8f, 0.1f, 0.1f, 0.1f, "B", styleSheet));
            content.Add(new TextElement(0.8f, 0.8f, 0.1f, 0.1f, "C", styleSheet));
            content.Add(new TextElement(0.1f, 0.8f, 0.1f, 0.1f, "D", styleSheet));

            sidebar.Add(new TextElement(25, 25, 150, 25, "1", styleSheet));
            sidebar.Add(new TextElement(25, 75, 150, 25, "2", styleSheet));
            sidebar.Add(new TextElement(25, 125, 150, 25, "3", styleSheet));
            sidebar.Add(new TextElement(25, 175, 150, 25, "4", styleSheet));
        }

        public override void Draw(GameTime gameTime)
        {
            _graphics.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
