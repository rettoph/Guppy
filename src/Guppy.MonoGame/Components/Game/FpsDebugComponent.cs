using Guppy.Attributes;
using Guppy.Common.Collections;
using Guppy.GUI;
using Guppy.MonoGame.Common;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Components.Game
{
    [AutoLoad]
    internal class FpsDebugComponent : GlobalComponent, IDebugComponent
    {

        private Buffer<double> _sampleBuffer = new Buffer<double>(20);
        private double _sampleSum = 0;

        private readonly IGui _gui;
        public FpsDebugComponent(IGui gui)
        {
            _gui = gui;
        }

        public void RenderDebugInfo(GameTime gameTime)
        {
            _sampleSum += gameTime.ElapsedGameTime.TotalSeconds;
            _sampleBuffer.Add(gameTime.ElapsedGameTime.TotalSeconds, out double removed);
            _sampleSum -= removed;

            var fps = 20.0 / _sampleSum;

            _gui.Text($"FPS: {fps.ToString("#,##0")}");
        }
    }
}
