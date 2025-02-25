﻿using Guppy.Core.Common.Attributes;
using Guppy.Game.ImGui.Common.Enums;
using Microsoft.Xna.Framework;

namespace Guppy.Game.ImGui.Common
{
    public interface IImGuiSystem
    {
        [RequireSequenceGroup<ImGuiSequenceGroupEnum>]
        void DrawImGui(GameTime gameTime);
    }
}