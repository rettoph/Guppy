﻿using Guppy.Resources.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Providers
{
    public interface IResourcePackTypeProvider
    {
        bool Provides(Type type);

        bool Load(IResourcePack pack, IEnumerable<IResourceDefinition> resources, bool strict);

        IResource? Get(string name);
    }
}
