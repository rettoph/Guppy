using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Configurations
{
    internal class ImGuiFontConfiguration
    {
        public ImGuiFontConfiguration(string trueTypeFontName, int sizePixels)
        {
            TrueTypeFontName = trueTypeFontName;
            SizePixels = sizePixels;
        }

        public string TrueTypeFontName { get; set; }
        public int SizePixels { get; set; }
    }
}
