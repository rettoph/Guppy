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

    public readonly struct SettingValue<T>(Setting<T> setting, T value) : ISettingValue, IRef<T>, IEquatable<SettingValue<T>>, IDisposable
        where T : notnull
    {
        private readonly UnmanagedReference<SettingValue<T>, T> _value = new(value);

        public readonly Setting<T> Setting = setting;

        public readonly T Value
        {
            get => this._value.Value;
            set => this._value.SetValue(value);
        }

        readonly Type IRef.Type => this.Setting.Type;

        readonly object? IRef.Value => this.Value;

        readonly ISetting ISettingValue.Setting => this.Setting;

        public SettingValue(Setting<T> setting) : this(setting, setting.DefaultValue)
        {

        }

        public readonly void Dispose()
        {
            this._value.Dispose();
        }

        public override readonly bool Equals(object? obj)
        {
            return this.Equals((Setting<T>)obj!);
        }

        public readonly bool Equals(SettingValue<T> other)
        {
            return this.Value.Equals(other.Value) && this.Setting.Equals(other.Setting);
        }

        public readonly bool Equals(ISettingValue? other)
        {
            return other is SettingValue<T> casted
                && this.Equals(casted);
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(this.Value, this.Setting.GetHashCode());
        }

        public static implicit operator T(SettingValue<T> setting)
        {
            return setting.Value;
        }

        public static bool operator ==(SettingValue<T> left, SettingValue<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SettingValue<T> left, SettingValue<T> right)
        {
            return !(left == right);
        }
    }
}