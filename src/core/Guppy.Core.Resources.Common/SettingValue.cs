using Guppy.Core.Common;
using Guppy.Core.Common.Utilities;

namespace Guppy.Core.Resources.Common
{
    public interface ISettingValue : IEquatable<ISettingValue>, IRef, IDisposable
    {
        ISetting Setting { get; }
    }

    public static class SettingValue
    {
        public static ISettingValue Create(ISetting setting)
        {
            Type settingValueType = typeof(SettingValue<>).MakeGenericType(setting.Type);
            ISettingValue settingValue = (ISettingValue)(Activator.CreateInstance(settingValueType, [setting]) ?? throw new NotImplementedException());

            return settingValue;
        }

        public static ISettingValue Create(ISetting setting, object value)
        {
            ThrowIf.Type.IsNotAssignableFrom(setting.Type, value.GetType());

            Type settingValueType = typeof(SettingValue<>).MakeGenericType(setting.Type);
            ISettingValue settingValue = (ISettingValue)(Activator.CreateInstance(settingValueType, [setting, value]) ?? throw new NotImplementedException());

            return settingValue;
        }
    }

    public struct SettingValue<T> : ISettingValue, IRef<T>, IEquatable<SettingValue<T>>, IDisposable
        where T : notnull
    {
        private readonly UnmanagedReference<SettingValue<T>, T> _value;

        public readonly Setting<T> Setting;

        public T Value
        {
            get => _value.Value;
            set => _value.SetValue(value);
        }

        Type IRef.Type => this.Setting.Type;
        object? IRef.Value => this.Value;

        ISetting ISettingValue.Setting => this.Setting;

        public SettingValue(Setting<T> setting) : this(setting, setting.DefaultValue)
        {

        }
        public SettingValue(Setting<T> setting, T value)
        {
            _value = new UnmanagedReference<SettingValue<T>, T>(value);
            this.Setting = setting;
        }

        public void Dispose()
        {
            _value.Dispose();
        }

        public override bool Equals(object? obj)
        {
            return this.Equals((Setting<T>)obj!);
        }

        public bool Equals(SettingValue<T> other)
        {
            return this.Value.Equals(other.Value) && this.Setting.Equals(other.Setting);
        }

        public bool Equals(ISettingValue? other)
        {
            return other is SettingValue<T> casted
                && this.Equals(casted);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Value, this.Setting.GetHashCode());
        }

        public static implicit operator T(SettingValue<T> setting)
        {
            return setting.Value;
        }
    }
}
