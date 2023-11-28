using Guppy.Common;
using Guppy.Common.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Input.Services
{
    internal class InputService : Broker<IInput>, IInputService
    {
        public InputService(IConfiguration<BrokerConfiguration<IInput>> configuration) : base(configuration)
        {
        }
    }
}
