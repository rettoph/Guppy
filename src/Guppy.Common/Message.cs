using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public abstract class Message : IMessage
    {
        public virtual Type PublishType { get; }

        public Message()
        {
            this.PublishType = this.GetType();
        }
    }
}
