using Guppy.Attributes;
using Guppy.GUI;
using Guppy.GUI.Providers;
using Guppy.GUI.Styling;
using Guppy.Resources.Providers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Components
{
    [AutoLoad]
    internal sealed class DebugComponent : IGuppyComponent, IGuiComponent
    {
        private readonly IStyler _styler;

        public DebugComponent(IStylerProvider stylers, IImFontProvider fonts, IResourceProvider resources)
        {
            _styler = stylers.Get(GUI.Resources.Styles.FullScreen);
        }

        public void Initialize(IGuppy guppy)
        {
        }

        public void DrawGui()
        {
            //using (_styler.Apply())
            //{
            //    ImGui.SetNextWindowSize(ImGui.GetMainViewport().Size);
            //
            //    if (ImGui.Begin(nameof(DebugComponent), ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoTitleBar))
            //    {
            //        ImGui.Text("test");
            //    }
            //
            //    ImGui.End();
            //}
        }
    }
}
