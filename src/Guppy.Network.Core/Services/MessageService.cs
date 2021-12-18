using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using Guppy.Network.MessageProcessors;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Services
{
    /// <summary>
    /// Simple service used to handle incoming or outgoing
    /// messages within a current scope.
    /// </summary>
    public sealed class MessageService : Service
    {
        #region Private Fields
        private NetworkProvider _network;
        private Dictionary<UInt16, MessageProcessor> _messageProcessors;
        private ILog _log;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            provider.Service(out _network);
            provider.Service(out _log);

            _messageProcessors = _network.MessageConfigurations
                .Where(mc => mc.Filter(provider, mc))
                .ToDictionary(
                    keySelector: mc => mc.Id.Value,
                    elementSelector: mc => mc.ProcessorFactory(provider));
        }

        protected override void Release()
        {
            base.Release();

            _network = default;
            _log = default;
        }
        #endregion

        #region Helper Methods
        public void ProcessIncoming(Message message)
        {
            if(_messageProcessors.TryGetValue(message.Configuration.Id.Value, out MessageProcessor processor))
            {
                processor.Process(message);
            }
            else
            {
                _log.Warn($"{nameof(MessageService)}::{nameof(ProcessIncoming)} - Recieved unprocessable message => '{message.Configuration.Name}'");
            }
        }
        #endregion
    }
}
