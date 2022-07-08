using Guppy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Services
{
    public interface ICommandService : IBroker<ICommandData>
    {
        void Invoke(string input);
    }
}
