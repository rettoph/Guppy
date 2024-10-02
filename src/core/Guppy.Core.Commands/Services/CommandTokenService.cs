using Guppy.Core.Commands.Common.Serialization.Commands;
using Guppy.Core.Commands.Common.Services;
using Guppy.Core.Common;
using System.Runtime.InteropServices;

namespace Guppy.Core.Commands.Services
{
    public sealed class CommandTokenService(IFiltered<ICommandTokenConverter> converters) : ICommandTokenService
    {
        private readonly ICommandTokenConverter[] _converters = converters.ToArray();
        private readonly Dictionary<Type, ICommandTokenConverter> _cache = new Dictionary<Type, ICommandTokenConverter>();

        public object? Deserialize(Type type, string token)
        {
            return this.GetConverter(type).Deserialize(type, token);
        }

        private ICommandTokenConverter GetConverter(Type type)
        {
            ref ICommandTokenConverter? converter = ref CollectionsMarshal.GetValueRefOrAddDefault(_cache, type, out bool exists);
            if (exists)
            {
                return converter!;
            }

            converter = _converters.First(x => x.AppliesTo(type));
            return converter;
        }
    }
}
