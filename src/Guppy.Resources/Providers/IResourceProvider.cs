﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Providers
{
    public interface IResourceProvider
    {
        T? Get<T>(Resource<T> resource)
            where T : notnull;

        bool TryGet<T>(Resource<T> resource, [MaybeNullWhen(false)] out T value) 
            where T : notnull;

        IEnumerable<(Resource, T)> GetAll<T>() where T : notnull;

        IResourceProvider Set<T>(Resource<T> resource, T value)
            where T : notnull;
    }
}
