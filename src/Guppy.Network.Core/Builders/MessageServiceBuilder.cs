using Guppy.Network.Configurations;
using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using Guppy.Network.Services;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Network.Builders
{
    public class MessageServiceBuilder : DataServiceBuilder<MessageService, MessageConfiguration>
    {
        public override MessageService Build()
        {
            // Mutate the registered configurations as needed...
            var configurations = this.configurations
                .PrioritizeBy(p => p.Name)
                .PrioritizeBy(p => p.Type)
                .AutoIncrementIds()
                .PrioritizeBy(p => p.Id)
                .ToList();

            // Determin the dynamic id size...
            DynamicIdSize idSize = configurations.GetIdSize();

            // Create a new PacketService instance...
            return new MessageService(
                idSize: idSize,
                configurations: configurations
                    .Select(pc => pc.Build(idSize)).ToArray());
        }
    }
}
