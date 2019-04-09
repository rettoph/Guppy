using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Enums
{
    public enum Alignment
    {
        Top = 1,
        VerticalCenter = 2,
        Bottom = 4,
        Left = 8,
        HorizontalCenter = 16,
        Right = 32,

        TopLeft = Alignment.Top | Alignment.Left,
        TopCenter = Alignment.Top | Alignment.HorizontalCenter,
        TopRight = Alignment.Top | Alignment.Right,

        CenterLeft = Alignment.VerticalCenter | Alignment.Left,
        CenterCenter = Alignment.VerticalCenter | Alignment.HorizontalCenter,
        Center = Alignment.VerticalCenter | Alignment.HorizontalCenter,
        CenterRight = Alignment.VerticalCenter | Alignment.Right,

        BottomLeft = Alignment.Bottom | Alignment.Left,
        BottomCenter = Alignment.Bottom | Alignment.HorizontalCenter,
        BottomRight = Alignment.Bottom | Alignment.Right
    }
}
