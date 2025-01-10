using System.Diagnostics.CodeAnalysis;
using Guppy.Game.Common;

namespace Guppy.Game
{
    public interface ISceneConfiguration
    {
        public abstract Type Type { get; }

        public ISceneConfiguration Set<T>(string key, T value)
            where T : notnull;

        public ref T Get<T>(string key, out bool exists);

        public T GetOrDefault<T>(string key, T defaultValue);

        public T? Get<T>(string key);

        public bool TryGet<T>(string key, [MaybeNullWhen(false)] out T value);

        public IEnumerable<KeyValuePair<string, object>> GetAllValues();
    }

    public interface ISceneConfiguration<out TScene> : ISceneConfiguration
        where TScene : class, IScene
    {
    }
}