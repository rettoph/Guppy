using Guppy.Extensions;
using Guppy.Loaders;
using Guppy.Network.Groups;
using Guppy.Network.Peers;
using Guppy.Network.Security;
using Guppy.UI.Elements;
using Guppy.UI.Entities;
using Guppy.UI.Enums;
using Guppy.UI.Loaders;
using Guppy.UI.Styles;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units.UnitValues;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Client.UI
{
    public class UserList : Element
    {
        private ScrollContainer _users;
        private TextElement _label;

        private Group _group;

        private Dictionary<User, FancyTextElement> _userLabels;

        public UserList(
            ClientPeer client,
            UnitRectangle outerBounds,
            Element parent,
            Stage stage,
            IServiceProvider provider)
            : base(
                  outerBounds,
                  parent,
                  stage,
                  provider.GetLoader<StyleLoader>().GetValue("user-list"))
        {
            _userLabels = new Dictionary<User, FancyTextElement>();

            _group = client.Groups.GetOrCreateById(Guid.Empty);
            _users = this.createElement<ScrollContainer>(0, 32, 1f, new UnitValue[] { 1f, -32 });
            _users.SetPadding(0, 0, 0, 0);
            _label = this.createElement<TextElement>(0, 0, 1f, 40, "Users");
            _label.SetPadding(8, 0, 8, 0);
            _label.Style.Set<Color>(StateProperty.TextColor, Color.White);
            _label.Style.Set<Alignment>(StateProperty.TextAlignment, Alignment.TopCenter);
            _label.Style.Set<Texture2D>(StateProperty.Background, provider.GetLoader<ContentLoader>().Get<Texture2D>("texture:ui:label:1"));

            
            foreach (User user in _group.Users)
                this.HandleUserAdded(this, user);

            _group.Users.Added += this.HandleUserAdded;
            _group.Users.Removed += this.HandleUserRemoved;
        }

        private void HandleUserAdded(object sender, User e)
        {
            var colorBytes = e.Get("color").Split(",");
            var label = _users.Items.CreateElement<FancyTextElement>(0, 0, 1f, 25);
            label.Add(e.Get("name"), new Color(Byte.Parse(colorBytes[0]), Byte.Parse(colorBytes[1]), Byte.Parse(colorBytes[2])));
            label.Style.Set<Alignment>(StateProperty.TextAlignment, Alignment.CenterLeft);
            label.SetPadding(2, 10, 2, 10);

            _userLabels.Add(e, label);
        }

        private void HandleUserRemoved(object sender, User e)
        {
            _users.Items.Remove(_userLabels[e]);
        }
    }
}
