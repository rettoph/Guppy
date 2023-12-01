using Guppy.Common;
using Guppy.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Input.Services
{
    public interface IInputService : IBroker<IInput>
    {
    }
}
