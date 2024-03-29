﻿using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Autofac;
using Guppy.Enums;

namespace Guppy
{
    [Service<IGuppyComponent>(ServiceLifetime.Scoped, true, tag: LifetimeScopeTags.GuppyScope)]
    public interface IGuppyComponent : ISequenceable<InitializeSequence>
    {
        void Initialize(IGuppy guppy);
    }
}
