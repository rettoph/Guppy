using Guppy.Common;
using Guppy.Input;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Input.Services
{
    public interface IButtonService : IUpdateSystem
    {
        void Set(string key, ButtonSource source);

        bool Get(string key);
    }
}
