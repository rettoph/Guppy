using Guppy.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Constants
{
    public static class Settings
    {
        public static Setting<bool> IsDebugWindowEnabled = Setting.Define<bool>(nameof(IsDebugWindowEnabled), true);
    }
}
