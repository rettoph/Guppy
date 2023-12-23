namespace Guppy.Resources.Constants
{
    public static class Settings
    {
        public static readonly Setting<string> Localization = Setting.Get<string>(nameof(Localization), Constants.Localization.en_US);
    }
}
