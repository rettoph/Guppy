﻿using Guppy.Attributes;
using Guppy.Common;
using Guppy.MonoGame.Services;
using Guppy.MonoGame.UI.Collections;
using Guppy.MonoGame.UI.Constants;
using ImGuiNET;
using ImPlotNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Num = System.Numerics;

namespace Guppy.MonoGame.UI.Services
{
    internal sealed class ImGuiDebuggerService : DefaultDebuggerService
    {
        private ImGuiBatch _imGuiBatch;
        private IntPtr _context;
        private ImFontPtr _font;
        private Num.Vector2 _buttonSize = new Num.Vector2(125, 35);
        private Num.Vector2 _emptyVector2 = new Num.Vector2(0, 0);
        private Num.Vector2 _menuPosition = new Num.Vector2(0, 0);
        private Num.Vector2 _menuSize = new Num.Vector2(0, 0);
        private Num.Vector2 _menuSpacing = new Num.Vector2(12, 12);
        private ImGuiDebuggerCollection _imGuiDebuggers;
        private GameWindow _window;

        public ImGuiDebuggerService(GameWindow window, ImGuiBatch imGuiBatch, IFiltered<IDebugger> debuggers) : base(debuggers, new ImGuiDebuggerCollection())
        {
            _imGuiBatch = imGuiBatch;
            _context = ImPlot.CreateContext();
            _font = imGuiBatch.Fonts[ImGuiFontConstants.DiagnosticsFont].Ptr;
            _window = window;
            _imGuiDebuggers = this.debuggers.GetManagedCollection<ImGuiDebuggerCollection>();
            _imGuiDebuggers.Initialize(_imGuiBatch);

            this.CleanMenuDimensions();

            _window.ClientSizeChanged += this.HandleClientSizeChanged;
        }

        private void CleanMenuDimensions()
        {
            _menuSize = new Num.Vector2(_buttonSize.X + (_menuSpacing.X * 2), _window.ClientBounds.Height);
            _menuPosition = new Num.Vector2(_window.ClientBounds.Width - _menuSize.X, 0);
        }

        public override void Draw(GameTime gameTime)
        {
            _imGuiBatch.Begin(gameTime);

            ImPlot.SetImGuiContext(_imGuiBatch.Context);
            ImPlot.SetCurrentContext(_context);

            ImGui.PushStyleColor(ImGuiCol.Text, Color.White.ToNumericsVector4());
            ImGui.PushFont(_font);

            ImGui.SetNextWindowPos(_menuPosition);
            ImGui.SetNextWindowSize(_menuSize);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, _menuSpacing);
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, _menuSpacing);
            if (ImGui.Begin("menu", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoBackground))
            {
                foreach(IImGuiDebugger debugger in _imGuiDebuggers)
                {
                    if(ImGui.Button(debugger.ButtonLabel, _buttonSize))
                    {
                        debugger.Toggle();
                    }
                }
            }
            ImGui.End();
            ImGui.PopStyleVar(2);

            base.Draw(gameTime);

            ImGui.PopFont();
            ImGui.PopStyleColor();

            _imGuiBatch.End();
        }

        private void HandleClientSizeChanged(object? sender, EventArgs e)
        {
            this.CleanMenuDimensions();
        }
    }
}
