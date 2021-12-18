using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Services
{
    public sealed class RoomService : Service
    {
        private static Byte PeerRoomId = 0;

        #region Private Fields
        private Dictionary<Byte, Room> _rooms;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _rooms = new Dictionary<Byte, Room>();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Get or create a new <see cref="Room"/> instance based on the
        /// given <paramref name="id"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Room Get(Byte id, ServiceProvider provider)
        {
            if (!_rooms.TryGetValue(0, out Room room))
            {
                room = new Room()
                {
                    Id = 0
                };

                _rooms.Add(0, room);
            }

            room.TryLinkScope(provider);
            return room;
        }
        /// <summary>
        /// Get or create a new <see cref="Room"/> instance based on the
        /// given <paramref name="id"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Room GetById(Byte id, ServiceProvider provider)
        {
            Debug.Assert(id == PeerRoomId, $"{nameof(Room)}::{nameof(GetById)} - {nameof(id)}:{PeerRoomId} is reserved for {nameof(Peer)} communications. Only mess with this {nameof(Room)} if you know what you're doing.");

            return this.Get(id, provider);
        }

        internal Room GetPeerRoom(ServiceProvider provider)
        {
            return this.Get(PeerRoomId, provider);
        }
        #endregion
    }
}
