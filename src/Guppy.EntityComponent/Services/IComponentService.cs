﻿using Guppy.EntityComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Services
{
    public interface IComponentService : IDisposable
    {
        T? Get<T>()
            where T : class, IComponent;

        bool Has<T>()
            where T : class, IComponent;

        bool TryGet<T>(out T? component)
            where T : class, IComponent;
    }
}