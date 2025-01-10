namespace Guppy.Core.Resources.Common.Constants
{
    public static class Settings
    {
        public static readonly Setting<string> Localization = Setting<string>.Get(nameof(Localization), "Region locale", Constants.Localization.en_US);
    }
}