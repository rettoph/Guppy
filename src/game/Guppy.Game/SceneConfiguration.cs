using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Extensions.System.Reflection;
using Guppy.Game.Common;
using Guppy.Game.Common.Attributes;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Guppy.Game
{
    internal abstract class SceneConfiguration : ISceneConfiguration
    {
        public abstract Type Type { get; }

        private readonly Dictionary<string, object> _values;

        internal SceneConfiguration()
        {
            _values = [];

            if (this.Type.TryGetAllCustomAttributes<SetSceneConfigurationAttribute>(true, out var attributes))
            {
                foreach (var attribute in attributes)
                {
                    attribute.SetValue(this);
                }
            }
        }

        public ISceneConfiguration Set<T>(string key, T value)
            where T : notnull
        {
            ref T valueRef = ref this.Get<T>(key, out _);
            valueRef = value;

            return this;
        }

        public ref T Get<T>(string key, out bool exists)
        {
            ref object? valueObject = ref CollectionsMarshal.GetValueRefOrAddDefault(_values, key, out exists);
            if (exists == false)
            {
                Ref<T> value = new(default!);
                valueObject = value;
                return ref value.Value;
            }

            if (valueObject is Ref<T> cached)
            {
                return ref cached.Value;
            }

            throw new InvalidOperationException($"{nameof(SceneConfiguration)}::{nameof(Get)} - Invalid type for key '{key}', expected {typeof(T).Name} but got {valueObject.GetType().GenericTypeArguments[0].Name}");
        }

        public T GetOrDefault<T>(string key, T defaultValue)
        {
            if (this.TryGet<T>(key, out T? value))
            {
                return value;
            }

            return defaultValue;
        }

        public T? Get<T>(string key)
        {
            return this.GetOrDefault<T>(key, default!);
        }

        public bool TryGet<T>(string key, [MaybeNullWhen(false)] out T value)
        {
            if (_values.TryGetValue(key, out object? valueRefObject) == false)
            {
                value = default;
                return false;
            }

            if (valueRefObject is IRef<T> valueRef)
            {
                value = valueRef.Value;
                return true;
            }

            if (valueRefObject is IRef objectRefObject && objectRefObject.Type.IsAssignableTo<T>())
            {
                value = (T?)objectRefObject.Value;
                return value is not null;
            }

            value = default;
            return false;
        }

        public IEnumerable<KeyValuePair<string, object>> GetAllValues()
        {
            return _values;
        }
    }

    internal sealed class SceneConfiguration<TScene> : SceneConfiguration, ISceneConfiguration<TScene>
        where TScene : class, IScene
    {
        public override Type Type => typeof(TScene);
    }
}
