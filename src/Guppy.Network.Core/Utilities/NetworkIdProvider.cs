using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Network.Interfaces;
using Guppy.Network.Enums;
using System.Collections.Concurrent;

namespace Guppy.Network.Utilities
{
    /// <summary>
    /// Simple helper used for defining a unique <see cref="IMagicNetworkEntity.NetworkId"/>
    /// value for instances created on a <see cref="NetworkAuthorization.Master"/>
    /// <see cref="HostType.Remote"/> host.
    /// </summary>
    public sealed class NetworkIdProvider
    {
        private ConcurrentQueue<UInt16> _surrendedIds = new ConcurrentQueue<UInt16>();
        private UInt16 _current = 0;

        /// <summary>
        /// Claim a new, unused Id.
        /// </summary>
        /// <returns></returns>
        public UInt16 ClaimId()
        {
            if (_surrendedIds.TryDequeue(out UInt16 networkId))
            { // Just re-use a returned id...
                return networkId;
            }

            return _current++;
        }

        /// <summary>
        /// Surrender an id so that it can be used again
        /// </summary>
        /// <param name="id"></param>
        public void SurrenderId(UInt16 id)
        {
            // This delay is arbitrary. The idea is to give it time before an id can be reused.
            // That way, the peers have time to sync-up and there wont be an unexpected collision.
            // Hopefully.
            Task.Run(async () =>
            {
                await Task.Delay(1000);
                _surrendedIds.Enqueue(id);
            });
        }
    }
}
