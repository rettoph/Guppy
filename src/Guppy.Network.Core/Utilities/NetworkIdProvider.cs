using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Network.Interfaces;
using Guppy.Network.Enums;

namespace Guppy.Network.Utilities
{
    /// <summary>
    /// Simple helper used for defining a unique <see cref="IMagicNetworkEntity.NetworkId"/>
    /// value for instances created on a <see cref="NetworkAuthorization.Master"/>
    /// <see cref="HostType.Remote"/> host.
    /// </summary>
    public sealed class NetworkIdProvider
    {
        private Queue<UInt16> _surrendedIds = new Queue<UInt16>();
        private UInt16 _current = 0;

        /// <summary>
        /// Claim a new, unused Id.
        /// </summary>
        /// <returns></returns>
        public UInt16 ClaimId()
        {
            if (_surrendedIds.Count > 0)
            { // Just re-use a returned id...
                return _surrendedIds.Dequeue();
            }

            return _current++;
        }

        /// <summary>
        /// Surrender an id so that it can be used again
        /// </summary>
        /// <param name="id"></param>
        public void SurrenderId(UInt16 id)
        {
            _surrendedIds.Enqueue(id);
        }
    }
}
