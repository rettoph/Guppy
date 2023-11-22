using Guppy.GUI.Styling;
using Guppy.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    public partial interface IGui
    {
        ImGuiFont GetFont(Resource<TrueTypeFont> ttf, int size);

        IStyler GetStyler(Resource<Style> style);
    }
}
