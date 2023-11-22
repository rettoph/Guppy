using Guppy.Attributes;
using Guppy.GUI;
using Guppy.GUI.Services;
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
        private readonly IImGuiService _imgui;

        public DebugComponent(IImGuiService imgui, IResourceProvider resources)
        {
            _imgui = imgui;
            _styler = imgui.GetStyler(GUI.Resources.Styles.FullScreen);
        }

        public void Initialize(IGuppy guppy)
        {
        }

        public void DrawGui()
        {
            using (_styler.Apply())
            {
                //_imgui.SetNextWindowSize(_imgui.GetMainViewport().Size);
                
                if (_imgui.Begin(nameof(DebugComponent), ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoTitleBar))
                {
                    _imgui.Text("test");
                }

                _imgui.End();
            }
        }
    }
}
