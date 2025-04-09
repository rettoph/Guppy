using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Guppy.Core.Files.Common;
using Guppy.Core.Serialization.Common.Services;

namespace Guppy.Core.Assets.Common.AssetTypes
{
    public abstract class AssetType<T> : IAssetType
        where T : notnull
    {
        public Type Type => typeof(T);

        public virtual string Name => this.Type.Name;

        public bool TryResolve(IAssetPack pack, string resourceName, string localization, JsonElement json)
        {
            AssetKey<T> resource = AssetKey<T>.Get(resourceName);

            if (this.TryResolve(resource, pack.RootDirectory, ref json, out T? value))
            {
                pack.Add(resource, localization, value);
                return true;
            }

            return false;
        }

        protected abstract bool TryResolve(AssetKey<T> resource, DirectoryPath root, ref JsonElement json, [MaybeNullWhen(false)] out T resolver);
    }

    public abstract class SimpleAssetType<T> : AssetType<T>
        where T : notnull
    {
        protected override bool TryResolve(AssetKey<T> resource, DirectoryPath root, ref JsonElement json, [MaybeNullWhen(false)] out T value)
        {
            string input = json.GetString() ?? string.Empty;
            return this.TryResolve(resource, root, input, out value);
        }

        protected abstract bool TryResolve(AssetKey<T> resource, DirectoryPath root, string input, out T value);
    }

    public class DefaultAssetType<T> : AssetType<T>
        where T : notnull
    {
        private readonly IJsonSerializationService _json;

        protected DefaultAssetType(IJsonSerializationService json)
        {
            this._json = json;
        }

        protected override bool TryResolve(AssetKey<T> resource, DirectoryPath root, ref JsonElement json, [MaybeNullWhen(false)] out T value)
        {
            value = this._json.Deserialize<T>(ref json, out bool success);

            return success;
        }
    }
}