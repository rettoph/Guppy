using Guppy.Attributes;
using Guppy.Common.Collections;
using Guppy.Game.ImGui;
using Guppy.Game;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Game.Components;

namespace Guppy.MonoGame.Components.Game
{
    [AutoLoad]
    internal class FpsDebugComponent : GlobalComponent, IDebugComponent
    {

        private Buffer<double> _sampleBuffer = new Buffer<double>(20);
        private double _sampleSum = 0;

        private readonly IImGui _imgui;
        public FpsDebugComponent(IImGui imgui)
        {
            _imgui = imgui;
        }

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
