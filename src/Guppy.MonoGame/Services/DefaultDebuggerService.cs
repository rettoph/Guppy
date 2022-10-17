using Guppy.Common;
using Guppy.MonoGame.Commands;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Services
{
    internal class DefaultDebuggerService : BaseWindowService, IDebuggerService
    {
        private IDebugger[] _debuggers;

        protected IDebugger[] debuggers => _debuggers;

        public override ToggleWindow.Windows Window => ToggleWindow.Windows.Debugger;

        public DefaultDebuggerService(IGlobal<IBus> bus, IEnumerable<IDebugger> debuggers) : base(bus, true)
        {
            _debuggers = debuggers.ToArray();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (IDebugger debugger in _debuggers)
            {
                debugger.Update(gameTime);
            }
        }

        protected override void InnerDraw(GameTime gameTime)
        {
            foreach (IDebugger debugger in _debuggers)
            {
                debugger.Draw(gameTime);
            }
        }
    }
}
