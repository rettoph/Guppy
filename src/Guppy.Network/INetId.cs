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
        static virtual INetId Zero => throw new NotImplementedException();
        static virtual byte SizeInBytes => throw new NotImplementedException();

        int Value { get; }

        void Write(NetDataWriter writer);

        INetId Next();

        static virtual INetId Read(NetDataReader reader)
        {
            throw new NotImplementedException();
        }
    }

    public interface INetId<T> : INetId
    {
        new T Value { get; }

        new INetId<T> Next();
    }
}
