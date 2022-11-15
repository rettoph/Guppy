using Guppy.Attributes;
using Guppy.Common;
using Guppy.MonoGame.Messages.Inputs;
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

        public override ToggleWindowInput.Windows Window => ToggleWindowInput.Windows.Debugger;

        public DefaultDebuggerService(IEnumerable<IDebugger> debuggers) : base(false)
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

        public override void Draw(GameTime gameTime)
        {
            foreach (IDebugger debugger in _debuggers)
            {
                debugger.Draw(gameTime);
            }
        }
    }
}
