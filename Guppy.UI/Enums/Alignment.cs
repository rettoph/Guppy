using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Enums
{
    public enum Alignment
    {
        Left             = 0,
        HorizontalCenter = 1,
        Right            = 2,
        Top              = 4,
        VerticleCenter   = 8,
        Bottom           = 16,
        TopLeft      = Alignment.Top | Alignment.Left,
        TopCenter    = Alignment.Top | Alignment.HorizontalCenter,
        TopRight     = Alignment.Top | Alignment.Right,
        CenterLeft   = Alignment.VerticleCenter | Alignment.Left,
        CenterCenter = Alignment.VerticleCenter | Alignment.HorizontalCenter,
        CenterRight  = Alignment.VerticleCenter | Alignment.Right,
        BottomLeft   = Alignment.Bottom | Alignment.Left,
        BottomCenter = Alignment.Bottom | Alignment.HorizontalCenter,
        BottomRight  = Alignment.Bottom | Alignment.Right,
    }
}
