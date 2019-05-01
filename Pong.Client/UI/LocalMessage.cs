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
    public class LocalMessage : FancyTextElement
    {
        public LocalMessage(String message, UnitRectangle outerBounds, Element parent, Stage stage, Style style = null) : base(outerBounds, parent, stage, style)
        {
            this.Add($"[{DateTime.Now.ToString("HH:mm:ss")}] ", Color.White);
            this.Add(message, Color.Gray);
        }
    }
}
