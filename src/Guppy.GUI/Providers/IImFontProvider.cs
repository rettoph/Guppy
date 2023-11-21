using Guppy.Resources;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Providers
{
    public interface IImFontProvider
    {
        ImFontPtr GetFontPtr(Resource<TrueTypeFont> font, int size);
    }
}
