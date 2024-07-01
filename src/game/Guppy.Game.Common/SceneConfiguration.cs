﻿using System.Diagnostics.CodeAnalysis;

namespace Guppy.Game.Common
{
    public interface ISceneConfiguration
    {
        public ISceneConfiguration Set<T>(string key, T value)
            where T : notnull;

        public ref T Get<T>(string key, out bool exists);

        public T? Get<T>(string key);

        public T GetOrDefault<T>(string key, T defaultValue);

        public bool TryGet<T>(string key, [MaybeNullWhen(false)] out T value);

        IEnumerable<KeyValuePair<string, object>> GetAllValues();
    }
}
