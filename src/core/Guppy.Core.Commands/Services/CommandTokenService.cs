using System.Runtime.InteropServices;
using Guppy.Core.Commands.Common.Serialization.Commands;
using Guppy.Core.Commands.Common.Services;
using Guppy.Core.Common;

namespace Guppy.Core.Commands.Services
{
    public sealed class CommandTokenService(IFiltered<ICommandTokenConverter> converters) : ICommandTokenService
    {
        private readonly ICommandTokenConverter[] _converters = [.. converters];
        private readonly Dictionary<Type, ICommandTokenConverter> _cache = [];

        public object? Deserialize(Type type, string token) => this.GetConverter(type).Deserialize(type, token);

        private ICommandTokenConverter GetConverter(Type type)
        {
            ref ICommandTokenConverter? converter = ref CollectionsMarshal.GetValueRefOrAddDefault(this._cache, type, out bool exists);
            if (exists)
            {
                return converter!;
            }

            converter = this._converters.First(x => x.AppliesTo(type));
            return converter;
        }
    }
}