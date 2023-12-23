namespace Guppy.Resources.Providers
{
    public interface ISettingProvider
    {
        SettingValue<T> Get<T>(Setting<T> setting)
            where T : notnull;

        void Set<T>(Setting<T> setting, T value)
            where T : notnull;

        void Save();
    }
}
