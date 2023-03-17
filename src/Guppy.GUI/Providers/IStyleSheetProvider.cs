using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Providers
{
    public interface IStyleSheetProvider
    {
        IStyleSheet Get(string name);
    }
}
