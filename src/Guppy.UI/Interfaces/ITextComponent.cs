using Guppy.UI.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Interfaces
{
    public interface ITextComponent : IComponent
    {
        #region Attributes
        SpriteFont Font { get; set; }
        Color Color { get; set; }
        String Text { get; set; }
        Alignment TextAlignment { get; set; }
        Boolean Inline { get; set; }
        #endregion

        #region Events
        event EventHandler<SpriteFont> OnFontChanged;
        event EventHandler<Color> OnColorChanged;
        event EventHandler<String> OnTextChanged;
        event EventHandler<Alignment> OnTextAlignmentChanged;
        event EventHandler<Boolean> OnInlineChanged;
        #endregion
    }
}
