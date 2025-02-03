using Guppy.Core.Resources.Common.Services;
using Guppy.Game.Graphics.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Game.ImGui.MonoGame
{
    public class MonoGameImGuiBatch : BaseImGuiBatch, IDisposable
    {
        // Graphics
        private readonly IGraphicsDevice _graphics;
        private readonly IGameWindow _window;
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
            IGameWindow window,
            IGraphicsDevice graphics,
            IResourceService resources) : base(resources)
        {
            this._window = window;
            this._graphics = graphics;
            this._loadedTextures = [];
            this._rasterizerState = new RasterizerState()
            {
                CullMode = CullMode.None,
                DepthBias = 0,
                FillMode = FillMode.Solid,
                MultiSampleAntiAlias = false,
                ScissorTestEnable = true,
                SlopeScaleDepthBias = 0

            };

            this._effect = new BasicEffect(this._graphics.Value);
            this._vertexData = [];
            this._vertexBuffer = new DynamicVertexBuffer(this._graphics.Value, DrawVertDeclaration.Declaration, this._vertexBufferSize, BufferUsage.None);
            this._indexData = [];
            this._indexBuffer = new DynamicIndexBuffer(this._graphics.Value, IndexElementSize.SixteenBits, this._indexBufferSize, BufferUsage.None);

            this._window.Value.TextInput += this.HandleTextInput;
        }

        public void Dispose()
        {
            this._window.Value.TextInput -= this.HandleTextInput;
        }

        public override nint BindTexture(byte[] pixels, int width, int height)
        {
            // Create and register the texture as an XNA texture
            var tex2d = new Texture2D(this._graphics.Value, width, height, false, SurfaceFormat.Color);
            tex2d.SetData(pixels);

            nint id = new(this._textureId++);

            this._loadedTextures.Add(id, tex2d);

            return id;
        }

        public override void UnbindTexture(nint textureId)
        {
            this._loadedTextures.Remove(textureId);
        }

        protected override void RenderDrawData(ImGuiNET.ImDrawDataPtr drawData)
        {
            // Setup render state: alpha-blending enabled, no face culling, no depth testing, scissor enabled, vertex/texcoord/color pointers
            var lastViewport = this._graphics.Value.Viewport;
            var lastScissorBox = this._graphics.Value.ScissorRectangle;

            this._graphics.Value.BlendFactor = Color.White;
            this._graphics.Value.BlendState = BlendState.NonPremultiplied;
            this._graphics.Value.RasterizerState = this._rasterizerState;
            this._graphics.Value.DepthStencilState = DepthStencilState.DepthRead;

            // Handle cases of screen coordinates != from framebuffer coordinates (e.g. retina displays)
            drawData.ScaleClipRects(this.IO.DisplayFramebufferScale);

            // Setup projection
            this._graphics.Value.Viewport = new Viewport(0, 0, this._graphics.Value.PresentationParameters.BackBufferWidth, this._graphics.Value.PresentationParameters.BackBufferHeight);

            if (drawData.TotalVtxCount == 0)
            {
                return;
            }

            this.UpdateBuffers(drawData);

            this.RenderCommandLists(drawData);

            // Restore modified state
            this._graphics.Value.Viewport = lastViewport;
            this._graphics.Value.ScissorRectangle = lastScissorBox;
        }

        private unsafe void UpdateBuffers(ImGuiNET.ImDrawDataPtr drawData)
        {
            // Expand buffers if we need more room
            if (drawData.TotalVtxCount > this._vertexBufferSize)
            {
                this._vertexBuffer?.Dispose();

                this._vertexBufferSize = (int)(drawData.TotalVtxCount * 1.5f);
                this._vertexBuffer = new DynamicVertexBuffer(this._graphics.Value, DrawVertDeclaration.Declaration, this._vertexBufferSize, BufferUsage.WriteOnly);
                this._vertexData = new byte[this._vertexBufferSize * DrawVertDeclaration.Size];
            }

            if (drawData.TotalIdxCount > this._indexBufferSize)
            {
                this._indexBuffer?.Dispose();

                this._indexBufferSize = (int)(drawData.TotalIdxCount * 1.5f);
                this._indexBuffer = new DynamicIndexBuffer(this._graphics.Value, IndexElementSize.SixteenBits, this._indexBufferSize, BufferUsage.WriteOnly);
                this._indexData = new byte[this._indexBufferSize * sizeof(ushort)];
            }

            // Copy ImGui's vertices and indices to a set of managed byte arrays
            int vtxOffset = 0;
            int idxOffset = 0;

            for (int n = 0; n < drawData.CmdListsCount; n++)
            {
                ImGuiNET.ImDrawListPtr cmdList = drawData.CmdLists[n];

                fixed (void* vtxDstPtr = &this._vertexData[vtxOffset * DrawVertDeclaration.Size])
                fixed (void* idxDstPtr = &this._indexData[idxOffset * sizeof(ushort)])
                {
                    Buffer.MemoryCopy((void*)cmdList.VtxBuffer.Data, vtxDstPtr, this._vertexData.Length, cmdList.VtxBuffer.Size * DrawVertDeclaration.Size);
                    Buffer.MemoryCopy((void*)cmdList.IdxBuffer.Data, idxDstPtr, this._indexData.Length, cmdList.IdxBuffer.Size * sizeof(ushort));
                }

                vtxOffset += cmdList.VtxBuffer.Size;
                idxOffset += cmdList.IdxBuffer.Size;
            }

            // Copy the managed byte arrays to the gpu vertex- and index buffers
            this._vertexBuffer.SetData(this._vertexData, 0, drawData.TotalVtxCount * DrawVertDeclaration.Size);
            this._indexBuffer.SetData(this._indexData, 0, drawData.TotalIdxCount * sizeof(ushort));
        }

        private unsafe void RenderCommandLists(ImGuiNET.ImDrawDataPtr drawData)
        {
            this._graphics.Value.SetVertexBuffer(this._vertexBuffer);
            this._graphics.Value.Indices = this._indexBuffer;

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

                    if (!this._loadedTextures.TryGetValue(drawCmd.TextureId, out Texture2D? value))
                    {
                        throw new InvalidOperationException($"Could not find a texture with id '{drawCmd.TextureId}', please check your bindings");
                    }

                    this._graphics.Value.ScissorRectangle = new Rectangle(
                        (int)drawCmd.ClipRect.X,
                        (int)drawCmd.ClipRect.Y,
                        (int)(drawCmd.ClipRect.Z - drawCmd.ClipRect.X),
                        (int)(drawCmd.ClipRect.W - drawCmd.ClipRect.Y)
                    );

                    var effect = this.UpdateEffect(value);

                    foreach (var pass in effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

#pragma warning disable CS0618 // // FNA does not expose an alternative method.
                        this._graphics.Value.DrawIndexedPrimitives(
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
        private BasicEffect UpdateEffect(Texture2D texture)
        {
            this._effect.World = Matrix.Identity;
            this._effect.View = Matrix.Identity;
            this._effect.Projection = Matrix.CreateOrthographicOffCenter(0f, this.IO.DisplaySize.X, this.IO.DisplaySize.Y, 0f, -1f, 1f);
            this._effect.TextureEnabled = true;
            this._effect.Texture = texture;
            this._effect.VertexColorEnabled = true;

            return this._effect;
        }

        protected override System.Numerics.Vector2 GetDisplaySize()
        {
            return new(this._graphics.Value.PresentationParameters.BackBufferWidth, this._graphics.Value.PresentationParameters.BackBufferHeight);
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