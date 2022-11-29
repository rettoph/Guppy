using Guppy.Common.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Collections
{
    internal sealed class ImGuiDebuggerCollection : SimpleManagedCollection<IImGuiDebugger>
    {
        public void Initialize(ImGuiBatch imGuiBatch)
        {
            foreach(var debugger in this)
            {
                debugger.Initialize(imGuiBatch);
            }
        }
    }
}
