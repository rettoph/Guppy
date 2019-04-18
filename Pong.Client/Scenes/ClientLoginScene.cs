using Guppy;
using Guppy.Collections;
using Guppy.Extensions;
using Guppy.Loaders;
using Guppy.Network.Peers;
using Guppy.Network.Security;
using Guppy.UI.Elements;
using Guppy.UI.Entities;
using Guppy.UI.Enums;
using Guppy.UI.Styles;
using Guppy.UI.Utilities.Units.UnitValues;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Library.Layers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Pong.Client.Scenes
{
    public class ClientLoginScene : Scene
    {
        private ClientPeer _client;
        private ContentLoader _content;
        private GraphicsDevice _graphicsDevice;
        private SceneCollection _scenes;

        public ClientLoginScene(SceneCollection scenes, GraphicsDevice graphicsDevice, ClientPeer client, IServiceProvider provider) : base(provider)
        {
            _scenes = scenes;
            _client = client;
            _content = provider.GetLoader<ContentLoader>();
            _graphicsDevice = graphicsDevice;
        }

        protected override void Initialize()
        {
            base.Initialize();

            var test = this.layers.Create<SimpleLayer>();
            test.Debug = false;

            var stage = this.entities.Create("ui:stage") as Stage;

            var form = new Form(new UnitValue[] { 0.5f, -(426/2) }, new UnitValue[] { 0.5f, -(300/2) }, 426, 300);
            stage.Content.Add(form);
            form.SetPadding(50, 50, 50, 50);
            form.Style.Set<Texture2D>(ElementState.Normal, StateProperty.Background, provider.GetLoader<ContentLoader>().Get<Texture2D>("texture:ui:login:form"));

            var labelStyle = new Style();
            labelStyle.Set<UnitValue>(GlobalProperty.PaddingTop, 5);
            labelStyle.Set<UnitValue>(GlobalProperty.PaddingRight, 5);
            labelStyle.Set<UnitValue>(GlobalProperty.PaddingBottom, 5);
            labelStyle.Set<UnitValue>(GlobalProperty.PaddingLeft, 5);
            labelStyle.Set<Color>(ElementState.Normal, StateProperty.TextColor, Color.White);
            labelStyle.Set<Alignment>(ElementState.Normal, StateProperty.TextAlignment, Alignment.CenterRight);

            form.Add(new TextElement("Name:", 0, 0, 100, 30, labelStyle));
            form.Add(new TextInput("", 100, 0, 226, 30));

            form.Add(new TextElement("Address:", 0, 40, 100, 30, labelStyle));
            form.Add(new TextInput("", 100, 40, 226, 30));

            form.Add(new TextElement("Port:", 0, 80, 100, 30, labelStyle));
            form.Add(new TextInput("", 100, 80, 226, 30));

            form.Add(new TextButton("Login", 0, 150, 1f, 50));
        }

        public override void Draw(GameTime gameTime)
        {
            _graphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}
