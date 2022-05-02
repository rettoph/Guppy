using Guppy.Attributes;
using Guppy.Network.Definitions;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.Definitions.NetMessengers
{
    [AutoLoad]
    internal sealed class TestNetMessageNetMessengerDefinition : NetMessengerDefinition<TestNetMessage>
    {
        public override DeliveryMethod DeliveryMethod => DeliveryMethod.ReliableOrdered;

        public override byte OutgoingChannel => 0;

        public override int OutgoingPriority => 0;
    }
}
