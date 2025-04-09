namespace Guppy.Core.Assets.Common.Services
{
    public interface ISettingService
    {
        void Save();

        ISettingValue this[ISetting setting] { get; }

        SettingValue<T> GetValue<T>(Setting<T> setting) where T : notnull;
        ISettingValue GetValue(ISetting setting);
    }
}