using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Services;
using System.Runtime.InteropServices;

namespace Guppy.Core.Network.Services
{
    internal sealed class NetGroupService(Func<byte, INetGroup> groupFactory) : INetGroupService
    {
        private readonly Dictionary<byte, INetGroup> _groups = new Dictionary<byte, INetGroup>();
        private readonly Func<byte, INetGroup> _groupFactory = groupFactory;

        public INetGroup GetById(byte id)
        {
            ref INetGroup? group = ref CollectionsMarshal.GetValueRefOrAddDefault(_groups, id, out bool exists);
            if (exists)
            {
                return group!;
            }

            group = _groupFactory(id);
            return group;
        }
    }
}
