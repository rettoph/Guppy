using Guppy.Gaming.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Guppy.Gaming.Content.Pipeline.Importers
{
    [ContentImporter(".ttf", DefaultProcessor = nameof(TrueTypeFontProcessor),
      DisplayName = "TrueTypeFont Importer - Guppy")]
    public class TrueTypeFontImporter : ContentImporter<byte[]>
    {
        public TrueTypeFontImporter()
        {

        }

        public override byte[] Import(string filename, ContentImporterContext context)
        {
            context.Logger.LogMessage("Importing TrueTypeFont file: {0}", filename);

            byte[] data = File.ReadAllBytes(filename);

            return data;
        }
    }
}
