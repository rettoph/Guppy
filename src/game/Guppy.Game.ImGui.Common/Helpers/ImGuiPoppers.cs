namespace Guppy.Game.ImGui.Common.Helpers
{
    public static class ImGuiPoppers
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
