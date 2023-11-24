﻿using Guppy.Attributes;
using Guppy.MonoGame.Constants;
using Guppy.Resources;
using Guppy.Resources.Loaders;
using Guppy.Resources.Providers;

namespace MonoGame.Loaders
{
    [AutoLoad]
    internal sealed class SettingLoader : ISettingLoader
    {
        public void Load(ISettingProvider settings)
        {
            settings.Register(Settings.IsDebugWindowEnabled, false);
        }
    }
}
