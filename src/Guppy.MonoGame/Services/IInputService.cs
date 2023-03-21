﻿using Guppy.Common;
using Guppy.MonoGame.Structs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Services
{
    public interface IInputService : IGameComponent, IUpdateable
    {
        void Set(string key, InputSource source);
    }
}
