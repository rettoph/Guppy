using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.MessageProcessors
{
    internal class EmptyIncomingMessageResponseMessageProcessor : MessageProcessor
    {
        public override void Process(Message message)
        {
            // throw new NotImplementedException();
        }

        public override void Dispose()
        {
            // throw new NotImplementedException();
        }

        public static EmptyIncomingMessageResponseMessageProcessor Factory(ServiceProvider provider)
        {
            return new EmptyIncomingMessageResponseMessageProcessor();
        }
    }
}
