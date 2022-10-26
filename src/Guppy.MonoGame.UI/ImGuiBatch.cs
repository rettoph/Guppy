﻿using Guppy.MonoGame.Services;
using Guppy.MonoGame.UI.Definitions;
using Guppy.MonoGame.UI.Providers;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.InteropServices;
using XnaColor = Microsoft.Xna.Framework.Color;
using Guppy.Resources.Providers;

namespace Guppy.MonoGame.UI
{
    /// <summary>
    /// ImGui renderer for use with XNA-likes (FNA & MonoGame)
    /// </summary>
    public sealed class ImGuiBatch
    {
        private GraphicsDevice _graphics;
        private GameWindow _window;
        private IResourceProvider _resources;

        // Graphics
        private BasicEffect _effect;
        private RasterizerState _rasterizerState;

        private byte[] _vertexData;
        private VertexBuffer _vertexBuffer;
        private int _vertexBufferSize;

        private byte[] _indexData;
        private IndexBuffer _indexBuffer;
        private int _indexBufferSize;

        // Textures
        private IntPtr? _fontTextureId;
        private Dictionary<IntPtr, Texture2D> _loadedTextures;

        private int _textureId;

        // Input
        private int _scrollWheelValue;
        private List<int> _keys = new List<int>();

        public readonly IntPtr Context;
        public readonly ImGuiIOPtr IO;
        public readonly IImGuiFontProvider Fonts;

        public ImGuiBatch(
            GameWindow window, 
            GraphicsDevice graphics,
            IResourceProvider resources,
            IEnumerable<IImGuiFontDefinition> fonts)
        {
            this.Context = ImGui.CreateContext();
            ImGui.SetCurrentContext(this.Context);
            this.IO = ImGui.GetIO();
            this.Fonts = new ImGuiFontProvider(resources, this.IO, fonts);

            _window = window;
            _graphics = graphics;
            _resources = resources;

            _loadedTextures = new Dictionary<IntPtr, Texture2D>();

            _rasterizerState = new RasterizerState()
            {
                CullMode = CullMode.None,
                DepthBias = 0,
                FillMode = FillMode.Solid,
                MultiSampleAntiAlias = false,
                ScissorTestEnable = true,
                SlopeScaleDepthBias = 0
            
            };

            _effect = new BasicEffect(_graphics);
            _vertexData = Array.Empty<byte>();
            _vertexBuffer = new VertexBuffer(_graphics, DrawVertDeclaration.Declaration, _vertexBufferSize, BufferUsage.None);
            _indexData = Array.Empty<byte>();
            _indexBuffer = new IndexBuffer(_graphics, IndexElementSize.SixteenBits, _indexBufferSize, BufferUsage.None);
            
            this.SetupInput();
            this.RebuildFontAtlas();
        }

        #region ImGuiRenderer
        public unsafe void RebuildFontAtlas()
        {
            // Get font texture from ImGui
            var io = ImGui.GetIO();
            io.Fonts.GetTexDataAsRGBA32(out byte* pixelData, out int width, out int height, out int bytesPerPixel);

            // Copy the data to a managed array
            var pixels = new byte[width * height * bytesPerPixel];
            unsafe { Marshal.Copy(new IntPtr(pixelData), pixels, 0, pixels.Length); }

            // Create and register the texture as an XNA texture
            var tex2d = new Texture2D(_graphics, width, height, false, SurfaceFormat.Color);
            tex2d.SetData(pixels);

            // Should a texture already have been build previously, unbind it first so it can be deallocated
            if (_fontTextureId.HasValue)
            {
                this.UnbindTexture(_fontTextureId.Value);
            }

            // Bind the new texture to an ImGui-friendly id
            _fontTextureId = this.BindTexture(tex2d);

            // Let ImGui know where to find the texture
            io.Fonts.SetTexID(_fontTextureId.Value);
            io.Fonts.ClearTexData(); // Clears CPU side texture data
        }

        /// <summary>
        /// Creates a pointer to a texture, which can be passed through ImGui calls such as <see cref="UI.Image" />. That pointer is then used by ImGui to let us know what texture to draw
        /// </summary>
        public IntPtr BindTexture(Texture2D texture)
        {
            var id = new IntPtr(_textureId++);

            _loadedTextures.Add(id, texture);

            return id;
        }

        /// <summary>
        /// Removes a previously created texture pointer, releasing its reference and allowing it to be deallocated
        /// </summary>
        public void UnbindTexture(IntPtr textureId)
        {
            _loadedTextures.Remove(textureId);
        }

        /// <summary>
        /// Sets up ImGui for a new frame, should be called at frame start
        /// </summary>
        public void Begin(GameTime gameTime, bool updateIO = true)
        {
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

            unsafe { RenderDrawData(ImGui.GetDrawData()); }
        }

        #endregion ImGuiRenderer

        #region Setup & Update

        /// <summary>
        /// Maps ImGui keys to XNA keys. We use this later on to tell ImGui what keys were pressed
        /// </summary>
        private void SetupInput()
        {
            _keys.Add(this.IO.KeyMap[(int)ImGuiKey.Tab] = (int)Keys.Tab);
            _keys.Add(this.IO.KeyMap[(int)ImGuiKey.LeftArrow] = (int)Keys.Left);
            _keys.Add(this.IO.KeyMap[(int)ImGuiKey.RightArrow] = (int)Keys.Right);
            _keys.Add(this.IO.KeyMap[(int)ImGuiKey.UpArrow] = (int)Keys.Up);
            _keys.Add(this.IO.KeyMap[(int)ImGuiKey.DownArrow] = (int)Keys.Down);
            _keys.Add(this.IO.KeyMap[(int)ImGuiKey.PageUp] = (int)Keys.PageUp);
            _keys.Add(this.IO.KeyMap[(int)ImGuiKey.PageDown] = (int)Keys.PageDown);
            _keys.Add(this.IO.KeyMap[(int)ImGuiKey.Home] = (int)Keys.Home);
            _keys.Add(this.IO.KeyMap[(int)ImGuiKey.End] = (int)Keys.End);
            _keys.Add(this.IO.KeyMap[(int)ImGuiKey.Delete] = (int)Keys.Delete);
            _keys.Add(this.IO.KeyMap[(int)ImGuiKey.Backspace] = (int)Keys.Back);
            _keys.Add(this.IO.KeyMap[(int)ImGuiKey.Enter] = (int)Keys.Enter);
            _keys.Add(this.IO.KeyMap[(int)ImGuiKey.Escape] = (int)Keys.Escape);
            _keys.Add(this.IO.KeyMap[(int)ImGuiKey.Space] = (int)Keys.Space);
            _keys.Add(this.IO.KeyMap[(int)ImGuiKey.A] = (int)Keys.A);
            _keys.Add(this.IO.KeyMap[(int)ImGuiKey.C] = (int)Keys.C);
            _keys.Add(this.IO.KeyMap[(int)ImGuiKey.V] = (int)Keys.V);
            _keys.Add(this.IO.KeyMap[(int)ImGuiKey.X] = (int)Keys.X);
            _keys.Add(this.IO.KeyMap[(int)ImGuiKey.Y] = (int)Keys.Y);
            _keys.Add(this.IO.KeyMap[(int)ImGuiKey.Z] = (int)Keys.Z);

            // MonoGame-specific //////////////////////
            _window.TextInput += (s, a) =>
            {
                if (a.Character == '\t') return;

                this.IO.AddInputCharacter(a.Character);
            };
            ///////////////////////////////////////////

            // FNA-specific ///////////////////////////
            //TextInputEXT.TextInput += c =>
            //{
            //    if (c == '\t') return;

            //    ImGui.GetIO().AddInputCharacter(c);
            //};
            ///////////////////////////////////////////

            //_io.Fonts.AddFontDefault();
        }

        /// <summary>
        /// Updates the <see cref="Effect" /> to the current matrices and texture
        /// </summary>
        private Effect UpdateEffect(Texture2D texture)
        {
            _effect.World = Matrix.Identity;
            _effect.View = Matrix.Identity;
            _effect.Projection = Matrix.CreateOrthographicOffCenter(0f, this.IO.DisplaySize.X, this.IO.DisplaySize.Y, 0f, -1f, 1f);
            _effect.TextureEnabled = true;
            _effect.Texture = texture;
            _effect.VertexColorEnabled = true;

            return _effect;
        }

        /// <summary>
        /// Sends XNA input state to ImGui
        /// </summary>
        private void UpdateInput()
        {
            var mouse = Mouse.GetState();
            var keyboard = Keyboard.GetState();

            for (int i = 0; i < _keys.Count; i++)
            {
                this.IO.KeysDown[_keys[i]] = keyboard.IsKeyDown((Keys)_keys[i]);
            }

            this.IO.KeyShift = keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift);
            this.IO.KeyCtrl = keyboard.IsKeyDown(Keys.LeftControl) || keyboard.IsKeyDown(Keys.RightControl);
            this.IO.KeyAlt = keyboard.IsKeyDown(Keys.LeftAlt) || keyboard.IsKeyDown(Keys.RightAlt);
            this.IO.KeySuper = keyboard.IsKeyDown(Keys.LeftWindows) || keyboard.IsKeyDown(Keys.RightWindows);

            this.IO.DisplaySize = new System.Numerics.Vector2(_graphics.PresentationParameters.BackBufferWidth, _graphics.PresentationParameters.BackBufferHeight);
            this.IO.DisplayFramebufferScale = new System.Numerics.Vector2(1f, 1f);

            this.IO.MousePos = new System.Numerics.Vector2(mouse.X, mouse.Y);

            this.IO.MouseDown[0] = mouse.LeftButton == ButtonState.Pressed;
            this.IO.MouseDown[1] = mouse.RightButton == ButtonState.Pressed;
            this.IO.MouseDown[2] = mouse.MiddleButton == ButtonState.Pressed;

            var scrollDelta = mouse.ScrollWheelValue - _scrollWheelValue;
            this.IO.MouseWheel = scrollDelta > 0 ? 1 : scrollDelta < 0 ? -1 : 0;
            _scrollWheelValue = mouse.ScrollWheelValue;
        }

        #endregion Setup & Update

        #region Internals

        /// <summary>
        /// Gets the geometry as set up by ImGui and sends it to the graphics device
        /// </summary>
        private void RenderDrawData(ImDrawDataPtr drawData)
        {
            // Setup render state: alpha-blending enabled, no face culling, no depth testing, scissor enabled, vertex/texcoord/color pointers
            var lastViewport = _graphics.Viewport;
            var lastScissorBox = _graphics.ScissorRectangle;

            _graphics.BlendFactor = XnaColor.White;
            _graphics.BlendState = BlendState.NonPremultiplied;
            _graphics.RasterizerState = _rasterizerState;
            _graphics.DepthStencilState = DepthStencilState.DepthRead;

            // Handle cases of screen coordinates != from framebuffer coordinates (e.g. retina displays)
            drawData.ScaleClipRects(this.IO.DisplayFramebufferScale);

            // Setup projection
            _graphics.Viewport = new Viewport(0, 0, _graphics.PresentationParameters.BackBufferWidth, _graphics.PresentationParameters.BackBufferHeight);

            UpdateBuffers(drawData);

            RenderCommandLists(drawData);

            // Restore modified state
            _graphics.Viewport = lastViewport;
            _graphics.ScissorRectangle = lastScissorBox;
        }

        private unsafe void UpdateBuffers(ImDrawDataPtr drawData)
        {
            if (drawData.TotalVtxCount == 0)
            {
                return;
            }

            // Expand buffers if we need more room
            if (drawData.TotalVtxCount > _vertexBufferSize)
            {
                _vertexBuffer?.Dispose();

                _vertexBufferSize = (int)(drawData.TotalVtxCount * 1.5f);
                _vertexBuffer = new VertexBuffer(_graphics, DrawVertDeclaration.Declaration, _vertexBufferSize, BufferUsage.None);
                _vertexData = new byte[_vertexBufferSize * DrawVertDeclaration.Size];
            }

            if (drawData.TotalIdxCount > _indexBufferSize)
            {
                _indexBuffer?.Dispose();

                _indexBufferSize = (int)(drawData.TotalIdxCount * 1.5f);
                _indexBuffer = new IndexBuffer(_graphics, IndexElementSize.SixteenBits, _indexBufferSize, BufferUsage.None);
                _indexData = new byte[_indexBufferSize * sizeof(ushort)];
            }

            // Copy ImGui's vertices and indices to a set of managed byte arrays
            int vtxOffset = 0;
            int idxOffset = 0;

            for (int n = 0; n < drawData.CmdListsCount; n++)
            {
                ImDrawListPtr cmdList = drawData.CmdListsRange[n];

                fixed (void* vtxDstPtr = &_vertexData[vtxOffset * DrawVertDeclaration.Size])
                fixed (void* idxDstPtr = &_indexData[idxOffset * sizeof(ushort)])
                {
                    Buffer.MemoryCopy((void*)cmdList.VtxBuffer.Data, vtxDstPtr, _vertexData.Length, cmdList.VtxBuffer.Size * DrawVertDeclaration.Size);
                    Buffer.MemoryCopy((void*)cmdList.IdxBuffer.Data, idxDstPtr, _indexData.Length, cmdList.IdxBuffer.Size * sizeof(ushort));
                }

                vtxOffset += cmdList.VtxBuffer.Size;
                idxOffset += cmdList.IdxBuffer.Size;
            }

            // Copy the managed byte arrays to the gpu vertex- and index buffers
            _vertexBuffer.SetData(_vertexData, 0, drawData.TotalVtxCount * DrawVertDeclaration.Size);
            _indexBuffer.SetData(_indexData, 0, drawData.TotalIdxCount * sizeof(ushort));
        }

        private unsafe void RenderCommandLists(ImDrawDataPtr drawData)
        {
            _graphics.SetVertexBuffer(_vertexBuffer);
            _graphics.Indices = _indexBuffer;

            int vtxOffset = 0;
            int idxOffset = 0;

            for (int n = 0; n < drawData.CmdListsCount; n++)
            {
                ImDrawListPtr cmdList = drawData.CmdListsRange[n];

                for (int cmdi = 0; cmdi < cmdList.CmdBuffer.Size; cmdi++)
                {
                    ImDrawCmdPtr drawCmd = cmdList.CmdBuffer[cmdi];

                    if (drawCmd.ElemCount == 0)
                    {
                        continue;
                    }

                    if (!_loadedTextures.ContainsKey(drawCmd.TextureId))
                    {
                        throw new InvalidOperationException($"Could not find a texture with id '{drawCmd.TextureId}', please check your bindings");
                    }

                    _graphics.ScissorRectangle = new Rectangle(
                        (int)drawCmd.ClipRect.X,
                        (int)drawCmd.ClipRect.Y,
                        (int)(drawCmd.ClipRect.Z - drawCmd.ClipRect.X),
                        (int)(drawCmd.ClipRect.W - drawCmd.ClipRect.Y)
                    );

                    var effect = UpdateEffect(_loadedTextures[drawCmd.TextureId]);

                    foreach (var pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

#pragma warning disable CS0618 // // FNA does not expose an alternative method.
                        _graphics.DrawIndexedPrimitives(
                            primitiveType: PrimitiveType.TriangleList,
                            baseVertex: (int)drawCmd.VtxOffset + vtxOffset,
                            minVertexIndex: 0,
                            numVertices: cmdList.VtxBuffer.Size,
                            startIndex: (int)drawCmd.IdxOffset + idxOffset,
                            primitiveCount: (int)drawCmd.ElemCount / 3
                        );
#pragma warning restore CS0618
                    }
                }

                vtxOffset += cmdList.VtxBuffer.Size;
                idxOffset += cmdList.IdxBuffer.Size;
            }
        }

        #endregion Internals
    }
}