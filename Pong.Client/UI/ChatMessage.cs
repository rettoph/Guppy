using Guppy.Network.Security;
using Guppy.UI.Elements;
using Guppy.UI.Entities;
using Guppy.UI.Styles;
using Guppy.UI.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Client.UI
{
    public class ChatMessage : FancyTextElement
    {
        public ChatMessage(User user, String message, UnitRectangle outerBounds, Element parent, Stage stage, Style style = null) : base(outerBounds, parent, stage, style)
        {
            var colorBytes = user.Get("color").Split(",");

            this.Add($"[{DateTime.Now.ToString("HH:mm:ss")}] ", Color.White);
            this.Add($"{user.Get("name")}: ", new Color(Byte.Parse(colorBytes[0]), Byte.Parse(colorBytes[1]), Byte.Parse(colorBytes[2])));
            this.Add(message, Color.White);
        }
    }
}
