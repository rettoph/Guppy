using Guppy.GUI.Styling;
using Guppy.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Providers
{
    public interface IStylerProvider
    {
        IStyler Get(Resource<Style> style);
    }
}
