using System.Drawing;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Graphics.Common
{
    public interface ICamera2D : ICamera
    {
        bool Center { get; set; }
        Vector2 Position { get; set; }
        float Zoom { get; set; }
        RectangleF ViewportBounds { get; }
    }
}