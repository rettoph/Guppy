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
using Pong.Client.UI;
using Pong.Library.Layers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Pong.Client.Scenes
{
    public class ClientLoginScene : Scene
    {
        private ClientPongGame _game;
        private ClientPeer _client;
        private ContentLoader _content;
        private GraphicsDevice _graphicsDevice;

        private TextInput _name;
        private TextInput _address;
        private TextInput _port;
        private TextButton _submit;
        private TextElement _message;

        public ClientLoginScene(ClientPongGame game, GraphicsDevice graphicsDevice, ClientPeer client, IServiceProvider provider) : base(provider)
        {
            _game = game;
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

            var form = stage.Content.CreateElement<Form>(new UnitValue[] { 0.5f, -(426 / 2) }, new UnitValue[] { 0.5f, -(300 / 2) }, 426, 300);
            form.SetPadding(50, 50, 50, 50);
            form.Style.Set<Texture2D>(ElementState.Normal, StateProperty.Background, provider.GetLoader<ContentLoader>().Get<Texture2D>("texture:ui:login:form"));

            var labelStyle = new Style();
            labelStyle.Set<UnitValue>(GlobalProperty.PaddingTop, 5);
            labelStyle.Set<UnitValue>(GlobalProperty.PaddingRight, 5);
            labelStyle.Set<UnitValue>(GlobalProperty.PaddingBottom, 5);
            labelStyle.Set<UnitValue>(GlobalProperty.PaddingLeft, 5);
            labelStyle.Set<Color>(ElementState.Normal, StateProperty.TextColor, Color.White);
            labelStyle.Set<Alignment>(ElementState.Normal, StateProperty.TextAlignment, Alignment.CenterRight);

            form.CreateElement<TextElement>(0, 0, 100, 30, "Name:", labelStyle);
            _name = form.CreateElement<TextInput>(100, 0, 226, 30, "Rettoph");

            form.CreateElement<TextElement>(0, 40, 100, 30, "Address:", labelStyle);
            _address = form.CreateElement<TextInput>(100, 40, 226, 30, "localhost");

            form.CreateElement<TextElement>(0, 80, 100, 30, "Port:", labelStyle);
            _port = form.CreateElement<TextInput>(100, 80, 226, 30, "1337");

            _submit = form.CreateElement<TextButton>(0, 150, 1f, 50, "Login");

            _message = stage.Content.CreateElement<TextElement>(0f, new UnitValue[] { 0.5f, -(300 / 2), 300 }, 1f, 30);
            _message.Style.Set<Alignment>(ElementState.Normal, StateProperty.TextAlignment, Alignment.Center);

            // Add post whitelist
            _port.CharWhitelist = new Regex(@"[0-9]");

            _submit.OnClicked += this.HandleSubmitClick;
            _client.OnStatusChanged += this.HandleClientStatusChanged;

            this.HandleSubmitClick(this, null);
        }

        public override void Draw(GameTime gameTime)
        {
            _graphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }

        private void logMessage()
        {

        }

        private void HandleSubmitClick(object sender, TextButton e)
        {
            if (_client.GetNetClient().ConnectionStatus == NetConnectionStatus.Disconnected)
            { // Only even try if we arent already tring to connect
                if (_name.Text.Trim() == String.Empty)
                {
                    _message.Style.Set<Color>(ElementState.Normal, StateProperty.TextColor, Color.Red);
                    _message.Text = "Error: Invalid name.";
                }
                else if (_address.Text.Trim() == String.Empty)
                {
                    _message.Style.Set<Color>(ElementState.Normal, StateProperty.TextColor, Color.Red);
                    _message.Text = "Error: Invalid address.";
                }
                else if (_port.Text.Trim() == String.Empty)
                {
                    _message.Style.Set<Color>(ElementState.Normal, StateProperty.TextColor, Color.Red);
                    _message.Text = "Error: Invalid port.";
                }
                else
                {
                    _message.Style.Set<Color>(ElementState.Normal, StateProperty.TextColor, Color.Cyan);
                    _message.Text = "Attempting to login...";

                    var user = new User();
                    user.Set("name", _name.Text);
                    user.Set("color", $"{Color.Red.R},{Color.Red.G},{Color.Red.B}");

                    _client.Connect(_address.Text, Int32.Parse(_port.Text), user);
                }
            }
        }

        private void HandleClientStatusChanged(object sender, NetIncomingMessage im)
        {
            switch ((NetConnectionStatus)im.ReadByte())
            {
                case NetConnectionStatus.Connected:
                    _message.Style.Set<Color>(ElementState.Normal, StateProperty.TextColor, Color.Green);
                    // Switch to a lobby scene instance...
                    _game.SetScene(_game.CreateScene<ClientLobbyScene>());
                    break;
                case NetConnectionStatus.Disconnected:
                    _message.Style.Set<Color>(ElementState.Normal, StateProperty.TextColor, Color.Red);
                    break;
                default:
                    _message.Style.Set<Color>(ElementState.Normal, StateProperty.TextColor, Color.Orange);
                    break;
            }
            _message.Text = im.ReadString();
        }
    }
}
