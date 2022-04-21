using Guppy.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Services
{
    public interface ICommandService : IBroker
    {
        public void Invoke(string command);
    }
}
