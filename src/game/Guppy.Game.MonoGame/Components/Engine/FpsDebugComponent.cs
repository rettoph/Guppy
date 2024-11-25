using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Collections;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Enums;
using Guppy.Game.ImGui.Common;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Components.Engine
{
    internal class FpsDebugComponent(IImGui imgui) : IEngineComponent, IDebugComponent
    {

        private readonly Buffer<double> _sampleBuffer = new(20);
        private double _sampleSum = 0;

        private readonly IImGui _imgui = imgui;

        [SequenceGroup<InitializeComponentSequenceGroup>(InitializeComponentSequenceGroup.Initialize)]
        public void Initialize(IGuppyEngine engine)
        {
            //
        }

        [SequenceGroup<DebugSequenceGroup>(DebugSequenceGroup.Debug)]
        public void DrawDebug(GameTime gameTime)
        {
            _sampleSum += gameTime.ElapsedGameTime.TotalSeconds;
            _sampleBuffer.Add(gameTime.ElapsedGameTime.TotalSeconds, out double removed);
            _sampleSum -= removed;

            var fps = 20.0 / _sampleSum;

            _imgui.Text($"FPS: {fps.ToString("#,##0")}");
        }
    }
}
