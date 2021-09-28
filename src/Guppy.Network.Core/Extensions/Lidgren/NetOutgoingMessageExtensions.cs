using Guppy.Extensions.System.Collections;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Network.Extensions.Lidgren
{
    public static class NetOutgoingMessageExtensions
    {
        #region Guid Methods
        public static void Write(this NetOutgoingMessage om, Guid guid)
        {
            om.Write(guid.ToByteArray());
        }
        #endregion

        #region Enum Methods
        public static void Write<T>(this NetOutgoingMessage om, T value)
            where T : Enum
                => om.Write(Convert.ToByte(value));
        #endregion

        #region IfNull Methods
        public static Boolean WriteIf(this NetOutgoingMessage om, Boolean value)
        {
            if (value)
            {
                om.Write(true);
                return true;
            }
            else
            {
                om.Write(false);
                return false;
            }
        }
        public static Boolean WriteExists(this NetOutgoingMessage om, Object value)
        {
            if (value is null)
            {
                om.Write(false);
                return false;
            }
            else
            {
                om.Write(true);
                return true;
            }
        }
        #endregion

        #region IEnumerable Methods
        public static void Write<T>(this NetOutgoingMessage om, IEnumerable<T> enumerable, Action<T, NetOutgoingMessage> writer)
        {
            om.Write(enumerable.Count());
            enumerable.ForEach(item => writer(item, om));
        }
        #endregion
    }
}
