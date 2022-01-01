using Guppy.Network;
using Guppy.Network.Interfaces;
using Guppy.Threading.Interfaces;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class ActionExtensions
    {
        public static Action<NetDataWriter, NetworkProvider, IData> ToIDataWriter<TData>(this Action<NetDataWriter, NetworkProvider, TData> writer)
            where TData : class, IData
        {
            void IDataWriter(NetDataWriter om, NetworkProvider network, IData data)
            {
                if(data is TData casted)
                {
                    writer(om, network, casted);

                    return;
                }

                throw new ArgumentException(nameof(data));
            }

            return IDataWriter;
        }
    }
}
