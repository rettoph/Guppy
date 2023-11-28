using Guppy.GUI.Styling.StylerValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Styling
{
    public interface IGuiStyle : IDisposable
    {
        void Push();

        void Pop();
    }
}
