using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Commands.Services
{
    public interface ICommandService
    {
        void Invoke(string input);
    }
}
