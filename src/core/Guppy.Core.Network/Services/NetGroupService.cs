using System.Runtime.InteropServices;

namespace Guppy.Core.Network.Common.Services
{
    internal sealed class NetGroupService : INetGroupService
    {
        private readonly Dictionary<byte, INetGroup> _groups;
        private readonly Func<byte, INetGroup> _groupFactory;

        public NetGroupService(Func<byte, INetGroup> groupFactory)
        {
            _groupFactory = groupFactory;
            _groups = new Dictionary<byte, INetGroup>();
        }

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
