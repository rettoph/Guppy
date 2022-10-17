using Guppy.Common;
using Guppy.MonoGame.Services;
using ImGuiNET;
using ImPlotNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Services
{
    internal sealed class ImGuiDebuggerService : DefaultDebuggerService
    {
        private ImGuiBatch _imGuiBatch;
        private IntPtr _context;

        public ImGuiDebuggerService(ImGuiBatch imGuiBatch, IGlobal<IBus> bus, IEnumerable<IDebugger> debuggers) : base(bus, debuggers)
        {
            _imGuiBatch = imGuiBatch;
            _context = ImPlot.CreateContext();

            foreach(IDebugger  debugger in this.debuggers)
            {
                if(debugger is IImGuiDebugger casted)
                {
                    casted.Initialize(_imGuiBatch);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _imGuiBatch.Begin(gameTime);

            ImPlot.SetImGuiContext(_imGuiBatch.Context);
            ImPlot.SetCurrentContext(_context);

            base.Draw(gameTime);

            _imGuiBatch.End();
        }
    }
}
