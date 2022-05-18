using Guppy.Attributes;
using Guppy.Gaming.Enums;
using Guppy.Gaming.Messages;
using Guppy.Threading;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Definitions.Commands
{
    [AutoLoad]
    public class Guppy : CommandDefinition
    {
        public override ICommandHandler? Publisher(IBroker broker)
        {
            return null;
        }

        [AutoLoad]
        public class Terminal : CommandDefinition
        {
            public override Type? Parent => typeof(Guppy);
            public override Option[] Options => new[]
            {
                new Option<TerminalAction>("--action")
            };

            public override ICommandHandler? Publisher(IBroker broker)
            {
                return CommandHandler.Create<TerminalAction>(action =>
                {
                    broker.PublishKnown<TerminalActionMessage>(new TerminalActionMessage(action));
                });
            }
        }

        [AutoLoad]
        public class Entities : CommandDefinition
        {
            public override Type? Parent => typeof(Guppy);

            public override ICommandHandler? Publisher(IBroker broker)
            {
                return CommandHandler.Create<TerminalAction>(action =>
                {
                    broker.PublishKnown<EntitiesMessage>(new EntitiesMessage());
                });
            }
        }
    }
}
