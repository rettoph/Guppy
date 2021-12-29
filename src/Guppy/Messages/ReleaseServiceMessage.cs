using Guppy.EntityComponent.Interfaces;
using Guppy.Threading.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Messages
{
    public sealed class ReleaseServiceMessage : IMessage
    {
        public IService Service { get; init; }
    }
}
