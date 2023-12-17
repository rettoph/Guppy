using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Game.ImGui.Helpers
{
    internal static class ImGuiPoppers
    {
        public class IdPopper : IDisposable
        {
            private readonly IImGui _imgui;

            public IdPopper(IImGui imgui)
            {
                _imgui = imgui;
            }

            public void Dispose()
            {
                _imgui.PopID();
            }
        }
    }
}
