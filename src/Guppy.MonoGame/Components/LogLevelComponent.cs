using Guppy.Attributes;
using Guppy.Common;
using Guppy.MonoGame.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Components
{
    internal class LogLevelComponent : GuppyComponent, ISubscriber<LogLevelCommand>
    {
        public void Process(in Guid messageId, in LogLevelCommand message)
        {
            throw new NotImplementedException();
        }
    }
}
