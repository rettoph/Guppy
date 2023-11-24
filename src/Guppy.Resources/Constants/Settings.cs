using Guppy.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Constants
{
    public static class Settings
    {
        public static readonly Setting<string> Localization = Setting.Define(nameof(Localization), Constants.Localization.Default);
    }
}
