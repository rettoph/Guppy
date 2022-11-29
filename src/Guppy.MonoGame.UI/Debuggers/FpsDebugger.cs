﻿using Guppy.Attributes;
using Guppy.Common.Collections;
using Guppy.MonoGame.UI;
using ImGuiNET;
using ImPlotNET;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Num = System.Numerics;

namespace Guppy.MonoGame.UI.Debuggers
{
    internal sealed class FpsDebugger : SimpleDebugger, IImGuiDebugger
    {
        private static readonly TimeSpan FpsInterval = TimeSpan.FromMilliseconds(100);
        private static readonly int FpsSize = (int)(TimeSpan.FromSeconds(10) / FpsInterval);

        private Buffer<double> _frames;
        private Buffer<double> _fps;
        private Buffer<double> _averages;
        private double _frameSum;
        private double _fpsSum;
        private double _fpsAverage;
        private double _current;
        private double _minimum;
        private double _maximum;
        private TimeSpan _lastAdded;
        private bool _graph;

        public string ButtonLabel { get; }

        public FpsDebugger()
        {
            this.ButtonLabel = "FPS";

            _frames = new Buffer<double>(256);
            _fps = new Buffer<double>(FpsSize);
            _averages = new Buffer<double>(FpsSize);

            this.IsEnabled = false;
            this.Visible = false;
        }

        public void Initialize(ImGuiBatch imGuiBatch)
        {
            //
        }

        public override void Draw(GameTime gameTime)
        {
            if(ImGui.Begin("FPS", ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoCollapse))
            {
                ImGui.Text($"Current: {_current.ToString("#,##0")}");
                ImGui.Text($"Average: {_fpsAverage.ToString("#,##0")}");
                ImGui.Text($"Minimum: {_minimum.ToString("#,##0")}");
                ImGui.Text($"Maximum: {_maximum.ToString("#,##0")}");

                if(ImGui.Button("Reset"))
                {
                    this.ResetBounds();
                }

                if (ImGui.Button("Toggle Graph"))
                {
                    _graph = !_graph;
                }

                if (_graph)
                {
                    ImPlot.SetNextAxisLimits(ImAxis.Y1, _minimum, _maximum, ImPlotCond.Always);
                    if (ImPlot.BeginPlot($"FPS Over Time", Num.Vector2.Zero))
                    {
                        ImPlot.PlotLine("Current", ref _fps.Items[0], _fps.Items.Length);
                        ImPlot.PlotLine("Average", ref _averages.Items[0], _averages.Items.Length);

                        ImPlot.EndPlot();
                    }
                }
            }

            ImGui.End();
        }

        private void ResetBounds()
        {
            _maximum = _current;
            _minimum = _current;

            _fpsSum = _fps.Sum();
            _fpsAverage = 0;
        }

        public override void Update(GameTime gameTime)
        {
            _frames.Add(gameTime.ElapsedGameTime.TotalMilliseconds, out double oldFrame);
            _frameSum += gameTime.ElapsedGameTime.TotalMilliseconds - oldFrame;

            _current = 1000f / (_frameSum / _frames.Length);

            _lastAdded += gameTime.ElapsedGameTime;

            while(_lastAdded >= FpsInterval)
            {
                _fps.Add(_current, out double oldFps);
                _lastAdded -= FpsInterval;

                _minimum = Math.Min(_minimum, _current);
                _maximum = Math.Max(_maximum, _current);

                _fpsSum += _current - oldFps;

                _fpsAverage = _fpsSum / _averages.Length;
                _averages.Add(_fpsAverage);
            }
        }

        public void Toggle()
        {
            this.IsEnabled = !this.IsEnabled;
            this.Visible = !this.Visible;
        }
    }
}
