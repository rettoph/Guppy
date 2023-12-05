using Guppy.Common;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Game.Input.Providers
{
    public interface IButtonProvider
    {
        IEnumerable<IInput> Update();
        void Clean(IEnumerable<IButton> buttons);
    }
}
