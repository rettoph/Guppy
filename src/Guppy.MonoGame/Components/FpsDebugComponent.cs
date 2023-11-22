using Guppy;
using Guppy.Attributes;
using Guppy.Common.Collections;
using Guppy.GUI;
using Guppy.MonoGame.Common;
using Guppy.MonoGame.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Components
{
    [AutoLoad]
    [GuppyFilter<GameLoop>]
    internal class FpsDebugComponent : GuppyComponent, IDebugComponent
    {
        private Buffer<double> _sampleBuffer = new Buffer<double>(20);
        private double _sampleSum = 0;

        public void RenderDebugInfo(IGui gui, GameTime gameTime)
        {
            _sampleSum += gameTime.ElapsedGameTime.TotalSeconds;
            _sampleBuffer.Add(gameTime.ElapsedGameTime.TotalSeconds, out double removed);
            _sampleSum -= removed;

            var fps = 20.0 / _sampleSum;

            gui.Text($"FPS: {fps.ToString("#,##0")}");
        }
    }
}
