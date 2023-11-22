using Guppy.Attributes;
using Guppy.GUI;
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
        private readonly IGui _gui;

        public DebugComponent(IGui gui, IResourceProvider resources)
        {
            _gui = gui;
            _styler = gui.GetStyler(GUI.Resources.Styles.FullScreen);
        }

        public void Initialize(IGuppy guppy)
        {
        }

        public void DrawGui()
        {
            using (_styler.Apply())
            {
                _gui.SetNextWindowSize(_gui.GetMainViewport().Size);
                
                if (_gui.Begin(nameof(DebugComponent), GuiWindowFlags.NoResize | GuiWindowFlags.NoMove | GuiWindowFlags.NoTitleBar))
                {
                    _gui.Button("test");
                }

                _gui.End();
            }
        }
    }
}
