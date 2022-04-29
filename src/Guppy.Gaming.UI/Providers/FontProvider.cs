using Guppy.Gaming.Content;
using Guppy.Gaming.Providers;
using Guppy.Gaming.UI.Definitions;
using Guppy.Providers;
using ImGuiNET;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.UI.Providers
{
    internal sealed class FontProvider : ResourceProvider<Font>, IFontProvider
    {
        private IContentProvider _content;
        private ImGuiRenderer _renderer;
        private GraphicsDevice _graphics;
        private Dictionary<string, Font> _fonts;
        private IntPtr? _fontTextureId;
        private ImGuiIOPtr _io;

        public unsafe FontProvider(
            IContentProvider content,
            ImGuiRenderer renderer,
            GraphicsDevice graphics,
            IEnumerable<FontDefinition> definitions)
        {
            _content = content;
            _renderer = renderer;
            _graphics = graphics;
            _io = ImGui.GetIO();

            _fonts = definitions.ToDictionary(x => x.Key, x => x.BuildFont(_content, _io));

            this.RebuildFontAtlas();
        }

        public static unsafe ImFontPtr Bind(byte* data, int dataSize, int sizePixels)
        {
            var io = ImGui.GetIO();
            return io.Fonts.AddFontFromMemoryTTF((IntPtr)data, dataSize, sizePixels);
        }

        public override IEnumerator<Font> GetEnumerator()
        {
            return _fonts.Values.GetEnumerator();
        }

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
            if (_fontTextureId.HasValue) _renderer.UnbindTexture(_fontTextureId.Value);

            // Bind the new texture to an ImGui-friendly id
            _fontTextureId = _renderer.BindTexture(tex2d);

            // Let ImGui know where to find the texture
            io.Fonts.SetTexID(_fontTextureId.Value);
            io.Fonts.ClearTexData(); // Clears CPU side texture data
        }

        public override bool TryGet(string key, [MaybeNullWhen(false)] out Font resource)
        {
            return _fonts.TryGetValue(key, out resource);
        }
    }
}
