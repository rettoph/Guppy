using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Services
{
    public interface IImguiObjectViewer
    {
        void Render(object value, string label = "", BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance);
    }
}
