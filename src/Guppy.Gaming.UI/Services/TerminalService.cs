using Guppy.Gaming.Services.Common;
using Guppy.Gaming.UI.Constants;
using Guppy.Gaming.UI.Providers;
using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNet = ImGuiNET.ImGui;

namespace Guppy.Gaming.UI.Services
{
    internal sealed class TerminalService : BaseTerminalService
    {
        private ImGuiRenderer _renderer;
        private ImFontPtr _font;

        public TerminalService(ImGuiRenderer stage, IFontProvider fonts)
        {
            _renderer = stage;
            // _renderer.RebuildFontAtlas();
            // _font = _renderer.BindFont(@"C:\Users\Anthony\source\repos\VoidHuntersRevived\libraries\Guppy\src\Guppy.Gaming\Content\Fonts\src\SpaceMono-Regular.ttf", 50);
            _font = fonts[FontConstants.DiagnosticsFont];
        }

        public override void WriteLine(string text, Microsoft.Xna.Framework.Color color)
        {
            throw new NotImplementedException();
        }

        protected override void Draw(GameTime gameTime)
        {
            _renderer.BeforeLayout(gameTime);

            ImGuiNet.PushFont(_font);
            ImGuiNet.Text("Hello, world!");
            ImGuiNet.PopFont();

            _renderer.AfterLayout();
        }

        protected override void Update(GameTime gameTime)
        {
            // throw new NotImplementedException();
        }
    }
}
