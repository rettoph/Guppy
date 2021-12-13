using Guppy.Network.Interfaces;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Extensions.System
{
    public static class ActionExtensions
    {
        public static Action<NetDataWriter, IPacket> ToIDataWriter<TData>(this Action<NetDataWriter, TData> writer)
            where TData : class, IPacket
        {
            void IDataWriter(NetDataWriter om, IPacket data)
            {
                if(data is TData casted)
                {
                    writer(om, casted);
                }

                throw new InvalidOperationException();
            }

            return IDataWriter;
        }
    }
}
