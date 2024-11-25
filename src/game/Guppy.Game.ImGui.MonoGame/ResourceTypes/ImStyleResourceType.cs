﻿using Guppy.Core.Resources.Common.ResourceTypes;
using Guppy.Core.Serialization.Common.Services;
using Guppy.Game.ImGui.Common.Styling;

namespace Guppy.Game.ImGui.MonoGame.ResourceTypes
{
    internal class ImStyleResourceType(IJsonSerializationService json) : DefaultResourceType<ImStyle>(json)
    {
    }
}