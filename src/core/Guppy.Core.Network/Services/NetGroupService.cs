using System.Runtime.InteropServices;
using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Services;

namespace Guppy.Core.Network.Services
{
    internal sealed class NetGroupService(Func<byte, INetGroup> groupFactory) : INetGroupService
    {
        private readonly Dictionary<byte, INetGroup> _groups = [];
        private readonly Func<byte, INetGroup> _groupFactory = groupFactory;

        public INetGroup GetById(byte id)
        {
            ref INetGroup? group = ref CollectionsMarshal.GetValueRefOrAddDefault(this._groups, id, out bool exists);
            if (exists)
            {
                return group!;
            }

            group = this._groupFactory(id);
            return group;
        }
    }
}