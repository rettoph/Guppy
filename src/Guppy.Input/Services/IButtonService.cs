using Guppy.Common;
using Guppy.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Input.Services
{
    public interface IButtonService : IGameComponent, IUpdateable
    {
        void Set(string key, ButtonSource source);

        bool Get(string key);
    }
}
