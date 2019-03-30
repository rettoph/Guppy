using Guppy;
using Guppy.Collections;
using Guppy.Extensions;
using Guppy.Loaders;
using Guppy.Network.Peers;
using Guppy.Network.Security;
using Guppy.UI.Elements;
using Guppy.UI.Entities;
using Guppy.UI.Enums;
using Guppy.UI.StyleSheets;
using Guppy.UI.Utilities.Units;
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
        private TextElement _loginMessage;
        private GraphicsDevice _graphicsDevice;
        private SceneCollection _scenes;

        private TextElement _name;
        private TextElement _address;
        private TextElement _port;

        public ClientLoginScene(SceneCollection scenes, GraphicsDevice graphicsDevice, ClientPeer client, IServiceProvider provider) : base(provider)
        {
            _scenes = scenes;
            _client = client;
            _content = provider.GetLoader<ContentLoader>();
            _graphicsDevice = graphicsDevice;

            _client.OnStatusChanged += this.HandleClientStatusChanged;
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.layers.Create<SimpleLayer>();

            var stage = this.entities.Create("ui:stage") as Stage;

            _loginMessage = stage.Content.Add(new TextElement(0.25f, new Unit[] { 0.45f, 100 }, 0.5f, 30, "")) as TextElement;

            var labelStyles = new StyleSheet();
            labelStyles.SetProperty<Alignment>(StyleProperty.TextAlignment, Alignment.CenterRight);
            labelStyles.SetProperty<Color>(StyleProperty.FontColor, Color.Black);

            var inputStyles = new StyleSheet();
            inputStyles.SetProperty<Alignment>(StyleProperty.TextAlignment, Alignment.CenterLeft);
            inputStyles.SetProperty<Color>(StyleProperty.FontColor, Color.Black);
            inputStyles.SetProperty<Texture2D>(StyleProperty.BackgroundImage, _content.Get<Texture2D>("texture:ui:login:input"));
            inputStyles.SetProperty<Texture2D>(ElementState.Active, StyleProperty.BackgroundImage, _content.Get<Texture2D>("texture:ui:login:input:active"));

            var buttonStyles = new StyleSheet();
            buttonStyles.SetProperty<Alignment>(StyleProperty.TextAlignment, Alignment.CenterCenter);
            buttonStyles.SetProperty<Color>(StyleProperty.FontColor, Color.White);
            buttonStyles.SetProperty<Texture2D>(StyleProperty.BackgroundImage, _content.Get<Texture2D>("texture:ui:login:button"));
            buttonStyles.SetProperty<Texture2D>(ElementState.Hovered, StyleProperty.BackgroundImage, _content.Get<Texture2D>("texture:ui:login:button:hovered"));
            buttonStyles.SetProperty<Texture2D>(ElementState.Pressed, StyleProperty.BackgroundImage, _content.Get<Texture2D>("texture:ui:login:button:pressed"));

            // Build login form
            var form = stage.Content.Add(new SimpleContainer(new Unit[] { 0.5f, -200 }, new Unit[] { 0.45f, -100 }, 400, 200)) as SimpleContainer;
            form.StyleSheet.SetProperty<Texture2D>(StyleProperty.BackgroundImage, _content.Get<Texture2D>("texture:ui:login:form"));

            form.Add(new TextElement(0.03f, 0.06f + (0.21f * 0), 150, 0.15f, "Name:", labelStyles));
            form.Add(new TextElement(0.03f, 0.06f + (0.21f * 1), 150, 0.15f, "Server Address:", labelStyles));
            form.Add(new TextElement(0.03f, 0.06f + (0.21f * 2), 150, 0.15f, "Server Port:", labelStyles));

            _name = form.Add(new TextInput(new Unit[] { 0.03f, 150 }, 0.06f + (0.21f * 0), new Unit[] { 0.94f, -150 }, 0.15f, "Tony", inputStyles));
            _address = form.Add(new TextInput(new Unit[] { 0.03f, 150 }, 0.06f + (0.21f * 1), new Unit[] { 0.94f, -150 }, 0.15f, "localhost", inputStyles));
            _port = form.Add(new TextInput(new Unit[] { 0.03f, 150 }, 0.06f + (0.21f * 2), new Unit[] { 0.94f, -150 }, 0.15f, "1337", inputStyles));

            var login = form.Add(new TextElement(0.03f, 0.69f, 0.94f, 0.25f, "Login", buttonStyles));
            login.OnMouseUp += this.HandleLoginClick;

            var list = stage.Content.Add<ScrollableList>(new ScrollableList(10, 0.1f, 175, 0.8f));

            for(Int32 i=0; i<100; i++)
            {
                list.Items.Add(new TextInput(0, 0, 1f, 30, i.ToString(), inputStyles));
            }
        }

        private void HandleLoginClick(object sender, Element e)
        {
            if(_name.Text == String.Empty)
            {
                _loginMessage.Text = "Please input a name...";
                _loginMessage.StyleSheet.SetProperty<Color>(StyleProperty.FontColor, Color.Red);
                _loginMessage.Dirty = true;
            }
            else if (_address.Text == String.Empty)
            {
                _loginMessage.Text = "Please input a server address...";
                _loginMessage.StyleSheet.SetProperty<Color>(StyleProperty.FontColor, Color.Red);
                _loginMessage.Dirty = true;
            }
            else if (_port.Text == String.Empty)
            {
                _loginMessage.Text = "Please input a server port...";
                _loginMessage.StyleSheet.SetProperty<Color>(StyleProperty.FontColor, Color.Red);
                _loginMessage.Dirty = true;
            }
            else if (!Int32.TryParse(_port.Text, out int n))
            {
                _loginMessage.Text = "Please input a valid server port...";
                _loginMessage.StyleSheet.SetProperty<Color>(StyleProperty.FontColor, Color.Red);
                _loginMessage.Dirty = true;
            }
            else
            { // Attempt to connect to the server
                _loginMessage.Text = "Attempting to login...";
                _loginMessage.StyleSheet.SetProperty<Color>(StyleProperty.FontColor, Color.LightBlue);
                _loginMessage.Dirty = true;

                var user = new User();
                user.Set("name", _name.Text);

                var thread = new Thread(new ThreadStart(() => {
                    try
                    {
                        _client.Connect(_address.Text, Int32.Parse(_port.Text), user);
                    }
                    catch (Exception err)
                    {
                        _loginMessage.Text = err.Message;
                        _loginMessage.StyleSheet.SetProperty<Color>(StyleProperty.FontColor, Color.Red);
                        _loginMessage.Dirty = true;
                    }
                }));
                thread.Start();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _graphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }

        #region Event Handlers
        private void HandleClientStatusChanged(object sender, NetIncomingMessage im)
        {

            switch((NetConnectionStatus)im.ReadByte())
            {
                case NetConnectionStatus.Connected:
                    _loginMessage.Text = im.ReadString();
                    _loginMessage.StyleSheet.SetProperty<Color>(StyleProperty.FontColor, Color.Green);
                    _loginMessage.Dirty = true;

                    // Remove the current scene from the game
                    var lobby = _scenes.Create<ClientLobbyScene>();
                    var transition = _scenes.Create<TransitionScene>();

                    transition.Set(this, lobby);
                    break;
                case NetConnectionStatus.Disconnected:
                    _loginMessage.Text = im.ReadString();
                    _loginMessage.StyleSheet.SetProperty<Color>(StyleProperty.FontColor, Color.Red);
                    _loginMessage.Dirty = true;
                    break;
                default:
                    _loginMessage.Text = im.ReadString();
                    _loginMessage.StyleSheet.SetProperty<Color>(StyleProperty.FontColor, Color.Orange);
                    _loginMessage.Dirty = true;
                    break;
            }
        }
        #endregion
    }
}
