using Guppy.Network.Interfaces;
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
        public static Action<NetDataWriter, IData> ToIDataWriter<TData>(this Action<NetDataWriter, TData> writer)
            where TData : class, IData
        {
            void IDataWriter(NetDataWriter om, IData data)
            {
                if(data is TData casted)
                {
                    writer(om, casted);

                    return;
                }

                throw new ArgumentException(nameof(data));
            }

            return IDataWriter;
        }
    }
}
