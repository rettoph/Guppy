using Guppy.Utilities.Delegaters;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Utilitites.Delegaters
{
    public sealed class MessageDelegater : CustomDelegater<String, NetIncomingMessage>
    {
        private ILogger _logger;

        public MessageDelegater(ILogger logger)
        {
            _logger = logger;
        }

        protected override void Invoke<T>(object sender, string key, T arg)
        {
#if DEBUG
            try
            {
                base.Invoke(sender, key, arg);
            }
            catch (KeyNotFoundException e)
            {
                _logger.LogWarning($"Unhandled Message recieved => '{key}'");
            }
#else
            base.Invoke(sender, key, arg);
#endif
        }
    }
}
