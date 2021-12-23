using Guppy.CommandLine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Messages.Commands
{
    public sealed class GuppyNetworkUsersCommand : ICommandData
    {
        public Int32? Id { get; init; }
    }
}
