using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public abstract class Message : IMessage
    {
        public Type Type { get; }

        public Message(Type type)
        {
            this.Type = type;
        }
        public Message()
        {
            this.Type = this.GetType();
        }
    }

    public abstract class Message<T> : Message
    {
        public Message() : base(typeof(T))
        {

        }
    }
}
