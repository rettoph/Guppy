﻿using Guppy.Core.Common.Attributes;
using Guppy.Core.Messaging.Common.Enums;
using Guppy.Engine.Common.Systems;
using Guppy.Game.ImGui.Common;
using Guppy.Game.ImGui.Common.Messages;
using Guppy.Game.Input.Common;

namespace Guppy.Game.ImGui.MonoGame.Systems.Engine
{
    public class ImGuiEventSystem(IImguiBatch imGuiBatch) : IEngineSystem, IInputSubscriber<ImGuiKeyEvent>, IInputSubscriber<ImGuiMouseButtonEvent>
    {
        private readonly IImguiBatch _imGuiBatch = imGuiBatch;

        [SequenceGroup<SubscriberSequenceGroupEnum>(SubscriberSequenceGroupEnum.PreProcess)]
        public void Process(in int messageId, ImGuiKeyEvent message)
        {
            this._imGuiBatch.SetKeyState(message);
        }

        [SequenceGroup<SubscriberSequenceGroupEnum>(SubscriberSequenceGroupEnum.PreProcess)]
        public void Process(in int messageId, ImGuiMouseButtonEvent message)
        {
            this._imGuiBatch.SetMouseButtonState(message);
        }
    }
}
