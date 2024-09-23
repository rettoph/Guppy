using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Collections;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Enums;
using Guppy.Game.ImGui.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Components.Engine
{
    [AutoLoad]
    [SequenceGroup<InitializeSequence>(InitializeSequence.Initialize)]
    internal class FpsDebugComponent : EngineComponent, IDebugComponent
    {

        private Buffer<double> _sampleBuffer = new Buffer<double>(20);
        private double _sampleSum = 0;

        private readonly IImGui _imgui;
        public FpsDebugComponent(IImGui imgui)
        {
            _imgui = imgui;
        }

        [SequenceGroup<DrawDebugComponentSequenceGroup>(DrawDebugComponentSequenceGroup.Draw)]
        public void RenderDebugInfo(GameTime gameTime)
        {
            _sampleSum += gameTime.ElapsedGameTime.TotalSeconds;
            _sampleBuffer.Add(gameTime.ElapsedGameTime.TotalSeconds, out double removed);
            _sampleSum -= removed;

            var fps = 20.0 / _sampleSum;

            _imgui.Text($"FPS: {fps.ToString("#,##0")}");
        }
    }
}
