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
        #region Private Fields
        private Dictionary<Byte, Room> _rooms;
        private ServiceProvider _provider;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _rooms = new Dictionary<Byte, Room>();
            _provider = provider.Root;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Process an incoming message
        /// </summary>
        /// <param name="message"></param>
        public void EnqueueIncoming(NetworkMessage message)
        {
            this.GetById(message.RoomId).EnqueueIncoming(message);
        }

        /// <summary>
        /// Get or create a new <see cref="Room"/> instance based on the
        /// given <paramref name="id"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Room GetById(Byte id)
        {
            if (!_rooms.TryGetValue(id, out Room room))
            {
                room = _provider.GetService<Room>((r, _, _) => r.Id = id);
                _rooms.Add(room.Id, room);
            }

            return room;
        }
        #endregion
    }
}
