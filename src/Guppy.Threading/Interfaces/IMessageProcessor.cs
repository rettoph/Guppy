using Guppy.EntityComponent.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading.Interfaces
{
    public interface IMessageProcessor<TMessage> : IService
        where TMessage : class, IMessage
    {
        void Process(TMessage message);
    }
}
