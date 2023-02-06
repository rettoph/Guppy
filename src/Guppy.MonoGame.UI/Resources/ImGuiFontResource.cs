using Guppy.MonoGame.UI.Configurations;
using Guppy.Resources;
using Guppy.Resources.Providers;
using ImGuiNET;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Guppy.MonoGame.UI.Resources.ImGuiFontResource;

namespace Guppy.MonoGame.UI.Resources
{
    internal class ImGuiFontResource : Resource<ImGuiFontFactory, ImGuiFontConfiguration>
    {

        public delegate ImGuiFont ImGuiFontFactory(ImGuiIOPtr io);

        private Lazy<IResourceProvider> _resources;

        public ImGuiFontConfiguration Configuration { get; set; }

        public ImGuiFontResource(string name, string trueTypeFontName, int sizePixels) : this(name, new ImGuiFontConfiguration(trueTypeFontName, sizePixels))
        {

        }
        public ImGuiFontResource(string name, ImGuiFontConfiguration configuration) : base(name)
        {
            _resources = default!;
            this.Configuration = configuration;
            this.Value = this.FactoryValue;
        }


        private unsafe ImGuiFont FactoryValue(ImGuiIOPtr io)
        {
            var ttf = _resources.Value.Get<TrueTypeFont>(this.Configuration.TrueTypeFontName);
            var ptr = ttf.Value.GetDataPtr();
            var fontPtr = io.Fonts.AddFontFromMemoryTTF(ptr, ttf.Value.GetDataSize(), this.Configuration.SizePixels);
            var font = new ImGuiFont(this.Name, ttf.Value, this.Configuration.SizePixels, fontPtr);

            return font;
        }

        public override void Initialize(string path, IServiceProvider services)
        {
            _resources = services.GetRequiredService<Lazy<IResourceProvider>>();
        }

        public override void Export(string path, IServiceProvider services)
        {
            // throw new NotImplementedException();
        }

        public override ImGuiFontConfiguration GetJson()
        {
            return this.Configuration;
        }
    }
}
