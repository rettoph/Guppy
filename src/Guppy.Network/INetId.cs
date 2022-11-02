using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public interface INetId : IEquatable<INetId>
    {
        void Write(NetDataWriter writer);

        INetId Next();

        // static abstract INetId Read(NetDataReader reader);
    }

    public interface INetId<T> : INetId
    {
        T Value { get; }

        new INetId<T> Next();

        // static abstract INetId<T> Read(NetDataReader reader);
        // static abstract INetId<T> Create(T value);
    }
}
