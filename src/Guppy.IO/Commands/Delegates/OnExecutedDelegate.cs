using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands.Delegates
{
    public delegate void OnExecutedDelegate(CommandInput input, IEnumerable<CommandResponse> responses);
}
