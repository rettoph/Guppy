using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;

namespace Guppy.Game.ImGui.Helpers
{
    public static class NumericsHelper
    {
        public static Num.Vector2 Convert(Vector2 value)
        {
            return Unsafe.As<Vector2, Num.Vector2>(ref value);
        }

        public static Num.Vector4 Convert(Vector4 value)
        {
            return Unsafe.As<Vector4, Num.Vector4>(ref value);
        }

        public static Num.Vector4 Convert(Color value)
        {
            return NumericsHelper.Convert(value.ToVector4());
        }
    }
}
