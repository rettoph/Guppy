﻿namespace Guppy.Core.Assets.Common.Constants
{
    public static class GuppyAssetSettings
    {
        public static readonly Setting<string> Localization = Setting<string>.Get(nameof(Localization), "Region locale", Constants.Localization.en_US);
    }
}