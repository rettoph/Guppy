﻿using Guppy.Common;
using Guppy.Common.Attributes;
using Guppy.Enums;
using Guppy.GUI.Messages;
using Guppy.GUI.Styling;
using Guppy.Input;
using Guppy.Input.Services;
using Guppy.Resources;
using Guppy.Resources.Providers;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    [Sequence<InitializeSequence>(InitializeSequence.PostInitialize)]
    internal abstract class BaseImGuiBatch : GlobalComponent, IInputSubscriber<ImGuiKeyEvent>, IInputSubscriber<ImGuiMouseButtonEvent>, IImguiBatch
    {
        public readonly TimeSpan StaleTime = TimeSpan.FromSeconds(1);

        private bool _dirtyFonts;
        private readonly Dictionary<(Resource<TrueTypeFont>, int), Ref<GuiFontPtr>> _fonts;

        private readonly IResourceProvider _resources;
        private DateTime _begin;

        // Textures
        private IntPtr? _fontTextureId;

        // Input
        private int _scrollWheelValue;
        private readonly List<int> _keys = new();
        private readonly Queue<char> _inputs;
        private readonly Queue<ImGuiKeyEvent> _keyEvents;
        private readonly Queue<ImGuiMouseButtonEvent> _mouseButtonEvents;

        public readonly IntPtr Context;
        public readonly ImGuiIOPtr IO;

        public bool Running { get; private set; }

        public bool Stale
        {
            get => DateTime.Now - _begin > StaleTime;
            set
            {
                if (value)
                {
                    _begin = DateTime.MinValue;
                }
                else
                {
                    _begin = DateTime.Now;
                }
            }
        }

        public BaseImGuiBatch(IResourceProvider resources)
        {
            this.Context = ImGui.CreateContext();
            ImGui.SetCurrentContext(this.Context);
            this.IO = ImGui.GetIO();

            this.IO.ConfigFlags |= ImGuiConfigFlags.DockingEnable;

            _fonts = new Dictionary<(Resource<TrueTypeFont>, int), Ref<GuiFontPtr>>();
            _mouseButtonEvents = new Queue<ImGuiMouseButtonEvent>();
            _keyEvents = new Queue<ImGuiKeyEvent>();
            _inputs = new Queue<char>();
            _begin = DateTime.MinValue;
            _resources = resources;
        }

        protected override void Initialize(IGlobalComponent[] components)
        {
            base.Initialize(components);

            _resources.Initialize(components);

            if(_fonts.Count > 0)
            {
                foreach(Ref<GuiFontPtr> font in _fonts.Values)
                {
                    font.Value.SetImFontPtr(this.IO.Fonts);
                }

                _dirtyFonts = true;
            }
        }

        #region ImGuiRenderer
        private unsafe void RebuildFontAtlas()
        {
            // Get font texture from ImGui
            ImGui.SetCurrentContext(this.Context);
            this.IO.Fonts.GetTexDataAsRGBA32(out byte* pixelData, out int width, out int height, out int bytesPerPixel);

            // Copy the data to a managed array
            var pixels = new byte[width * height * bytesPerPixel];
            unsafe { Marshal.Copy(new IntPtr(pixelData), pixels, 0, pixels.Length); }

            // Should a texture already have been build previously, unbind it first so it can be deallocated
            if (_fontTextureId.HasValue)
            {
                this.UnbindTexture(_fontTextureId.Value);
            }

            // Bind the new texture to an ImGui-friendly id
            _fontTextureId = this.BindTexture(pixels, width, height);

            // Let ImGui know where to find the texture
            this.IO.Fonts.SetTexID(_fontTextureId.Value);
            this.IO.Fonts.ClearTexData(); // Clears CPU side texture data
        }

        /// <summary>
        /// Creates a pointer to a texture, which can be passed through ImGui calls such as <see cref="UI.Image" />. That pointer is then used by ImGui to let us know what texture to draw
        /// </summary>
        public abstract IntPtr BindTexture(byte[] pixels, int width, int height);

        /// <summary>
        /// Removes a previously created texture pointer, releasing its reference and allowing it to be deallocated
        /// </summary>
        public abstract void UnbindTexture(IntPtr textureId);

        /// <summary>
        /// Sets up ImGui for a new frame, should be called at frame start
        /// </summary>
        public void Begin(GameTime gameTime)
        {
            if(this.Running)
            {
                throw new Exception();
            }

            this.Running = true;

            if (this.Stale)
            {
                _inputs.Clear();
                _keyEvents.Clear();
                _mouseButtonEvents.Clear();
            }

            if(_dirtyFonts)
            {
                this.RebuildFontAtlas();
                _dirtyFonts = false;
            }

            _begin = DateTime.Now;

            ImGui.SetCurrentContext(this.Context);

            this.IO.DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateInput();

            ImGui.NewFrame();
        }

        /// <summary>
        /// Asks ImGui for the generated geometry data and sends it to the graphics pipeline, should be called after the UI is drawn using ImGui.** calls
        /// </summary>
        public void End()
        {
            ImGui.Render();

            unsafe {
                var drawData = ImGui.GetDrawData();

                // Handle cases of screen coordinates != from framebuffer coordinates (e.g. retina displays)
                drawData.ScaleClipRects(this.IO.DisplayFramebufferScale);

                RenderDrawData(drawData); 
            }

            this.Running = false;
        }

        #endregion ImGuiRenderer

        #region Setup & Update
        /// <summary>
        /// Sends XNA input state to ImGui
        /// </summary>
        private void UpdateInput()
        {
            while (_inputs.TryDequeue(out var input))
            {
                this.IO.AddInputCharacter(input);
            }

            while (_keyEvents.TryDequeue(out var keyEvent))
            {
                this.IO.AddKeyEvent(keyEvent.Key, keyEvent.Down);
            }

            while (_mouseButtonEvents.TryDequeue(out var mouseButtonEvent))
            {
                this.IO.AddMouseButtonEvent(mouseButtonEvent.Button, mouseButtonEvent.Down);
            }

            var mouse = Mouse.GetState();

            this.IO.DisplaySize = this.GetDisplaySize();
            this.IO.DisplayFramebufferScale = new Num.Vector2(1f, 1f);

            this.IO.MousePos = new Num.Vector2(mouse.X, mouse.Y);

            var scrollDelta = mouse.ScrollWheelValue - _scrollWheelValue;
            this.IO.MouseWheel = scrollDelta > 0 ? 1 : scrollDelta < 0 ? -1 : 0;
            _scrollWheelValue = mouse.ScrollWheelValue;
        }

        protected abstract Num.Vector2 GetDisplaySize();

        protected void Input(char value)
        {
            _inputs.Enqueue(value);
        }
        #endregion Setup & Update

        #region Internals

        /// <summary>
        /// Gets the geometry as set up by ImGui and sends it to the graphics device
        /// </summary>
        protected abstract void RenderDrawData(ImDrawDataPtr drawData);
        #endregion Internals

        public void Process(in Guid messageId, ImGuiKeyEvent message)
        {
            if (this.Stale)
            {
                return;
            }

            _keyEvents.Enqueue(message);
        }

        public void Process(in Guid messageId, ImGuiMouseButtonEvent message)
        {
            if (this.Stale)
            {
                return;
            }

            _mouseButtonEvents.Enqueue(message);
        }

        public Ref<GuiFontPtr> GetFont(Resource<TrueTypeFont> ttf, int size)
        {
            ref Ref<GuiFontPtr>? font = ref CollectionsMarshal.GetValueRefOrAddDefault(_fonts, (ttf, size), out bool exists);

            if(exists)
            {
                return font!;
            }

            font = new Ref<GuiFontPtr>(default!);

            ResourceValue<TrueTypeFont> ttfInstance = _resources.Get(ttf);
            font.Value = new GuiFontPtr(ttfInstance, size);

            if (this.Ready)
            {
                font.Value.SetImFontPtr(this.IO.Fonts);
            }

            _dirtyFonts = true;

            return font;
        }
    }
}