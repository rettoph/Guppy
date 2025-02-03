using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Collections;
using Guppy.Core.Common.Enums;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Systems;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common.Systems;
using Guppy.Game.ImGui.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Components.Engine
{
    public class FpsDebugSystem(IImGui imgui) : IEngineSystem, IDebugSystem
    {

        private readonly Buffer<double> _sampleBuffer = new(20);
        private double _sampleSum = 0;

        private readonly IImGui _imgui = imgui;

        [SequenceGroup<InitializeSystemSequenceGroupEnum>(InitializeSystemSequenceGroupEnum.Initialize)]
        public void Initialize(IGuppyEngine engine)
        {
            //
        }

        [SequenceGroup<DebugSequenceGroupEnum>(DebugSequenceGroupEnum.Debug)]
        public void DrawDebug(GameTime gameTime)
        {
            this._sampleSum += gameTime.ElapsedGameTime.TotalSeconds;
            this._sampleBuffer.Add(gameTime.ElapsedGameTime.TotalSeconds, out double removed);
            this._sampleSum -= removed;

            double fps = 20.0 / this._sampleSum;

            this._imgui.Text($"FPS: {fps:#,##0}");
        }
    }
}