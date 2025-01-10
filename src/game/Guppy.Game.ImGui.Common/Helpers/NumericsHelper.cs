using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace Guppy.Game.ImGui.Common.Helpers
{
    public static class NumericsHelper
    {
        public static Num.Vector2 Convert(Vector2 value) => Unsafe.As<Vector2, Num.Vector2>(ref value);

        public static Num.Vector4 Convert(Vector4 value) => Unsafe.As<Vector4, Num.Vector4>(ref value);

        public static Num.Vector4 Convert(Color value) => NumericsHelper.Convert(value.ToVector4());
    }
}