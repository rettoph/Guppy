namespace Guppy.Game.ImGui.Common.Helpers
{
    public static class ImGuiPoppers
    {
        public class IdPopper(IImGui imgui) : IDisposable
        {
            private readonly IImGui _imgui = imgui;

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
            public void Dispose()
            {
                this._imgui.PopID();
            }
        }
    }
}