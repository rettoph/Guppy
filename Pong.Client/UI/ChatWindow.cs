using Guppy.Extensions;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Groups;
using Guppy.Network.Peers;
using Guppy.Network.Security;
using Guppy.UI.Elements;
using Guppy.UI.Entities;
using Guppy.UI.Loaders;
using Guppy.UI.Styles;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units.UnitValues;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Client.UI
{
    public class ChatWindow : Element
    {
        private ScrollContainer _messages;
        private TextInput _input;
        private User _server;

        private Group _group;

        public ChatWindow(ClientPeer client, UnitRectangle outerBounds, Element parent, Stage stage, IServiceProvider provider, Style style = null) : base(outerBounds, parent, stage, style)
        {
            var styleLoader = provider.GetLoader<StyleLoader>();

            _messages = this.createElement<ScrollContainer>(0, 0, 1f, new UnitValue[] { 1f, -20 });
            _input = this.createElement<TextInput>(0, new UnitValue[] { 1f, -20 }, 1f, 20, styleLoader.GetValue("chat-input"));
            _server = new User();
            _group = client.Groups.GetOrCreateById(Guid.Empty);

            _server.Set("name", "Server");
            _server.Set("color", $"{Color.Cyan.R},{Color.Cyan.G},{Color.Cyan.B}");

            // Start 100% scrolled
            _messages.ScrollTo(1f);
            _messages.Items.MaxItems = 25;

            // Add a simple welcome message
            this.Add($"Welcome, {client.CurrentUser.Get("name")}!");

            _input.OnEnter += this.HandleInputEnter;
            _group.MessageHandler.Add("chat", this.HandleChatMessage);
        }

        public void Add(User user, String message)
        {
            _messages.Items.CreateElement<ChatMessage>(0, 0, 1f, 25, user, message);
        }
        public void Add(String message)
        {
            _messages.Items.CreateElement<ChatMessage>(0, 0, 1f, 25, _server, message);
        }

        private void HandleInputEnter(object sender, string e)
        {
            if (_input.Text.Trim() != String.Empty)
            {
                var om = _group.CreateMessage("chat");
                om.Write(_input.Text.Trim());
                _group.SendMesssage(om);

                _input.Text = "";
            }
        }

        private void HandleChatMessage(NetIncomingMessage obj)
        {
            var sender = _group.Users.GetById(obj.ReadGuid());
            var content = obj.ReadString();

            this.Add(sender, content);
        }
    }
}
