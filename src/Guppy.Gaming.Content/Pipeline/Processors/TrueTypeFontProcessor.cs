using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Guppy.Gaming.Content.Pipeline.Processors
{
    [ContentProcessor(DisplayName = "TrueTypeFont Processor - Guppy")]
    public class TrueTypeFontProcessor : ContentProcessor<byte[], TrueTypeFont>
    {
        public override TrueTypeFont Process(byte[] input, ContentProcessorContext context)
        {
            try
            {
                TrueTypeFont font = new TrueTypeFont(input);

                return font;
            }
            catch (Exception ex)
            {
                context.Logger.LogMessage("Error {0}", ex);
                throw;
            }
        }
    }
}
