using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public class BrokerConfiguration
    {
        internal sealed class MessageAliasConfiguration
        {
            public required Type Message { get; init; }
            public required Type Alias { get; init; }
            public required bool Inheritable { get; init; }
        }

        private IList<MessageAliasConfiguration> _messageAliases { get; }

        public BrokerConfiguration()
        {
            _messageAliases = new List<MessageAliasConfiguration>();
        }

        public BrokerConfiguration AddMessageAlias(Type message, Type alias, bool inheritable)
        {
            ThrowIf.Type.IsNotAssignableFrom<IMessage>(message);
            ThrowIf.Type.IsNotAssignableFrom<IMessage>(alias);

            _messageAliases.Add(new MessageAliasConfiguration()
            {
                Message = message,
                Alias = alias,
                Inheritable = inheritable
            });

            return this;
        }

        public BrokerConfiguration AddMessageAlias<TMessage, TAlias>(bool inheritable)
            where TMessage : IMessage
            where TAlias : IMessage
        {
            return this.AddMessageAlias(typeof(TMessage), typeof(TAlias), inheritable);
        }

        public Type[] GetAliases(IMessage message)
        {
            HashSet<Type> aliases = new();
            aliases.Add(message.Type);

            foreach(var alias in _messageAliases)
            {
                if(alias.Message == message.Type)
                {
                    aliases.Add(alias.Alias);
                    continue;
                }

                if(alias.Inheritable && alias.Message.IsAssignableFrom(message.Type))
                {
                    aliases.Add(alias.Alias);
                    continue;
                }
            }

            return aliases.ToArray();
        }
    }
}
