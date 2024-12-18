﻿using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Enums;

namespace Guppy.Game.Common.Components
{
    public interface ISceneComponent
    {

    }

    public interface ISceneComponent<TScene> : ISceneComponent
        where TScene : IScene
    {
        [RequireSequenceGroup<InitializeComponentSequenceGroup>]
        void Initialize(TScene scene);
    }
}
