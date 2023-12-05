using Guppy.Common;
using Guppy.Game.ImGui.Messages;
using Guppy.Input;
using Guppy.Resources.Providers;
using Guppy.Resources;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Game.ImGui
{
    internal class MonoGameImGuiBatch : BaseImGuiBatch, IDisposable
    {
        // Graphics
        private readonly GraphicsDevice _graphics;
        private readonly GameWindow _window;
        private readonly BasicEffect _effect;
        private readonly RasterizerState _rasterizerState;

        private byte[] _vertexData;
        private DynamicVertexBuffer _vertexBuffer;
        private int _vertexBufferSize;

        private byte[] _indexData;
        private DynamicIndexBuffer _indexBuffer;
        private int _indexBufferSize;

        private readonly Dictionary<IntPtr, Texture2D> _loadedTextures;
        private int _textureId;

        public MonoGameImGuiBatch(
            GameWindow window,
            GraphicsDevice graphics,
            IResourceProvider resources) : base(resources)
        {
            _window = window;
            _graphics = graphics;
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
            _vertexBuffer = new DynamicVertexBuffer(_graphics, DrawVertDeclaration.Declaration, _vertexBufferSize, BufferUsage.None);
            _indexData = Array.Empty<byte>();
            _indexBuffer = new DynamicIndexBuffer(_graphics, IndexElementSize.SixteenBits, _indexBufferSize, BufferUsage.None);

            _window.TextInput += this.HandleTextInput;
        }

        public void Dispose()
        {
            _window.TextInput -= this.HandleTextInput;
        }

        public override nint BindTexture(byte[] pixels, int width, int height)
        {
            // Create and register the texture as an XNA texture
            var tex2d = new Texture2D(_graphics, width, height, false, SurfaceFormat.Color);
            tex2d.SetData(pixels);

            var id = new IntPtr(_textureId++);

            _loadedTextures.Add(id, tex2d);

            return id;
        }

        public override void UnbindTexture(nint textureId)
        {
            _loadedTextures.Remove(textureId);
        }

        protected override void RenderDrawData(ImGuiNET.ImDrawDataPtr drawData)
        {
            // Setup render state: alpha-blending enabled, no face culling, no depth testing, scissor enabled, vertex/texcoord/color pointers
            var lastViewport = _graphics.Viewport;
            var lastScissorBox = _graphics.ScissorRectangle;

            _graphics.BlendFactor = Color.White;
            _graphics.BlendState = BlendState.NonPremultiplied;
            _graphics.RasterizerState = _rasterizerState;
            _graphics.DepthStencilState = DepthStencilState.DepthRead;

            // Handle cases of screen coordinates != from framebuffer coordinates (e.g. retina displays)
            drawData.ScaleClipRects(this.IO.DisplayFramebufferScale);

            // Setup projection
            _graphics.Viewport = new Viewport(0, 0, _graphics.PresentationParameters.BackBufferWidth, _graphics.PresentationParameters.BackBufferHeight);

            if (drawData.TotalVtxCount == 0)
            {
                return;
            }

            UpdateBuffers(drawData);

            RenderCommandLists(drawData);

            // Restore modified state
            _graphics.Viewport = lastViewport;
            _graphics.ScissorRectangle = lastScissorBox;
        }

        private unsafe void UpdateBuffers(ImGuiNET.ImDrawDataPtr drawData)
        {
            // Expand buffers if we need more room
            if (drawData.TotalVtxCount > _vertexBufferSize)
            {
                _vertexBuffer?.Dispose();

                _vertexBufferSize = (int)(drawData.TotalVtxCount * 1.5f);
                _vertexBuffer = new DynamicVertexBuffer(_graphics, DrawVertDeclaration.Declaration, _vertexBufferSize, BufferUsage.WriteOnly);
                _vertexData = new byte[_vertexBufferSize * DrawVertDeclaration.Size];
            }

            if (drawData.TotalIdxCount > _indexBufferSize)
            {
                _indexBuffer?.Dispose();

                _indexBufferSize = (int)(drawData.TotalIdxCount * 1.5f);
                _indexBuffer = new DynamicIndexBuffer(_graphics, IndexElementSize.SixteenBits, _indexBufferSize, BufferUsage.WriteOnly);
                _indexData = new byte[_indexBufferSize * sizeof(ushort)];
            }

            // Copy ImGui's vertices and indices to a set of managed byte arrays
            int vtxOffset = 0;
            int idxOffset = 0;

            for (int n = 0; n < drawData.CmdListsCount; n++)
            {
                ImGuiNET.ImDrawListPtr cmdList = drawData.CmdLists[n];

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

        private unsafe void RenderCommandLists(ImGuiNET.ImDrawDataPtr drawData)
        {
            _graphics.SetVertexBuffer(_vertexBuffer);
            _graphics.Indices = _indexBuffer;

            int vtxOffset = 0;
            int idxOffset = 0;

            for (int n = 0; n < drawData.CmdListsCount; n++)
            {
                ImGuiNET.ImDrawListPtr cmdList = drawData.CmdLists[n];

                for (int cmdi = 0; cmdi < cmdList.CmdBuffer.Size; cmdi++)
                {
                    ImGuiNET.ImDrawCmdPtr drawCmd = cmdList.CmdBuffer[cmdi];

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

        protected override System.Numerics.Vector2 GetDisplaySize()
        {
            return new System.Numerics.Vector2(_graphics.PresentationParameters.BackBufferWidth, _graphics.PresentationParameters.BackBufferHeight);
        }

        private void HandleTextInput(object? sender, TextInputEventArgs e)
        {
            if (this.Stale)
            {
                return;
            }

            if (e.Character == '\t')
            {
                return;
            }

            this.Input(e.Character);
        }
    }
}
