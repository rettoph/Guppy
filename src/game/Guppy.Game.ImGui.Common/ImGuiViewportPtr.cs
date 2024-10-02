using Microsoft.Xna.Framework;

namespace Guppy.Game.ImGui.Common
{
    public partial struct ImGuiViewportPtr
    {
        public Vector2 Size => this.Value.Size;

        public static bool operator ==(ImGuiViewportPtr left, ImGuiViewportPtr right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ImGuiViewportPtr left, ImGuiViewportPtr right)
        {
            return !(left == right);
        }
    }
}
