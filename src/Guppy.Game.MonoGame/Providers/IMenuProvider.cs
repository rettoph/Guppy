using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Game.MonoGame.Providers
{
    public interface IMenuProvider
    {
        void Add(Menu menu);

        Menu Get(string name);
    }
}
