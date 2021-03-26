using Guppy.Utilities;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Network.Extensions.Lidgren
{
    public static class NetIncomingMessageExtensions
    {
        #region Guid Methods
        public static Guid PeekGuid(this NetIncomingMessage im)
        {
            return new Guid(im.PeekBytes(16));
        }
        public static Guid ReadGuid(this NetIncomingMessage im)
        {
            return new Guid(im.ReadBytes(16));
        }
        #endregion

        #region Enum Methods
        public static T ReadEnum<T>(this NetIncomingMessage im)
            where T : Enum
        {
            var byteVal = im.ReadByte();
            return EnumHelper.GetValues<T>().First(v => Convert.ToByte(v) == byteVal);
        }
        #endregion

        #region IfNull Methods
        public static Boolean ReadExists(this NetIncomingMessage im)
        {
            return im.ReadBoolean();
        }
        #endregion

        #region IEnumerable Methods
        public static IEnumerable<T> ReadEnumerable<T>(this NetIncomingMessage im, Func<NetIncomingMessage, T> reader)
        {
            var count = im.ReadInt32();
            for (Int32 i = 0; i < count; i++)
                yield return reader(im);
        }
        #endregion

        #region Recycle Methods
        public static void Recycle(this NetIncomingMessage im)
            => im.SenderConnection.Peer.Recycle(im);
        #endregion
    }
}
