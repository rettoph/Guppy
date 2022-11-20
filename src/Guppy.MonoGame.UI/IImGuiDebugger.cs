using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI
{
    public interface IImGuiDebugger : IDebugger
    {
        string Label { get; }

        bool Open { get; set; }

        void Initialize(ImGuiBatch imGuiBatch);
    }
}
