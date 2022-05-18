﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent
{
    public interface IComponent : IDisposable
    {
        void Initialize(IEntity entity);
        void Uninitilaize();
    }
}
