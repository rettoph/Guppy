namespace Guppy.Game.ImGui.Common.Helpers
{
    internal static class ImGuiPoppers
    {
        public class IdPopper(IImGui imgui) : IDisposable
        {
            private readonly IImGui _imgui = imgui;

            public void Dispose()
            {
                _imgui.PopID();
            }
        }
    }
}
