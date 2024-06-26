﻿using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Game.Common.Attributes;

namespace Guppy.Game.Common.Components
{
    [SceneFilter<IScene>]
    [Service(ServiceLifetime.Scoped, ServiceRegistrationFlags.RequireAutoLoadAttribute | ServiceRegistrationFlags.AsImplementedInterfaces)]
    public interface ISceneComponent
    {
        void Initialize();
    }
}
