using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Entities.UI.Interfaces
{
    public interface ITextElement
    {
        BaseElement.Alignment TextAlignment { get; set; }
        String Text { get; set; }
        Color TextColor { get; set; }
        SpriteFont Font { get; set; }
    }
}
