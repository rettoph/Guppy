﻿using Guppy.Core.Common;
using Guppy.Core.Resources.Common;
using Guppy.Game.ImGui.Common.Messages;
using Microsoft.Xna.Framework;

namespace Guppy.Game.ImGui.Common
{
    public interface IImguiBatch
    {
        bool Running { get; }

        /// <summary>
        /// Sets up ImGui for a new frame, should be called at frame start
        /// </summary>
        void Begin(GameTime gameTime);

        /// <summary>
        /// Asks ImGui for the generated geometry data and sends it to the graphics pipeline, should be called after the UI is drawn using ImGui.** calls
        /// </summary>
        void End();

        Ref<ImFontPtr> GetFont(Resource<TrueTypeFont> ttf, int size);

        void SetKeyState(ImGuiKeyEvent data);
        void SetMouseButtonState(ImGuiMouseButtonEvent data);
    }
}