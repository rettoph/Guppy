using Guppy.Common;
using Guppy.Resources;
using Guppy.Resources.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Resources
{
    [PolymorphicJsonType("TrueTypeFont")]
    internal sealed class TrueTypeFontResource : Resource<TrueTypeFont, string>
    {
        public string File { get; }

        public TrueTypeFontResource(string name, string file) : base(name)
        {
            File = file;
        }

        public override void Initialize(string path, IServiceProvider services)
        {
            var file = Path.Combine(path, File);

            if (!System.IO.File.Exists(file))
            {
                throw new FileNotFoundException(file);
            }

            using (var stream = System.IO.File.OpenRead(file))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                Value = new TrueTypeFont(buffer);
            }
        }

        public override string GetJson()
        {
            return File;
        }

        public override void Export(string path, IServiceProvider services)
        {
            throw new NotImplementedException();
        }
    }
}
