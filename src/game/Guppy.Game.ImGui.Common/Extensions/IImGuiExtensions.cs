using Microsoft.Xna.Framework;

namespace Guppy.Game.ImGui.Common.Extensions
{
    public static class IImGuiExtensions
    {
        public static void KeyValue(this IImGui imgui, string key, string value, bool sameLine = true, Vector4? keyColor = null, Vector4? valueColor = null)
        {
            if (keyColor is null)
            {
                imgui.Text(key);
            }
            else
            {
                imgui.TextColored(keyColor.Value, key);
            }

            if (sameLine == true)
            {
                imgui.SameLine();
            }


            if (valueColor is null)
            {
                imgui.Text(value);
            }
            else
            {
                imgui.TextColored(valueColor.Value, value);
            }
        }
    }
}
