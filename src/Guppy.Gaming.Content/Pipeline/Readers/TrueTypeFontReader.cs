using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Content.Pipeline.Readers
{
    public class TrueTypeFontReader : ContentTypeReader<TrueTypeFont>
    {
        protected override TrueTypeFont Read(ContentReader input, TrueTypeFont existingInstance)
        {
            var dataLength = input.ReadInt32();
            var data = input.ReadBytes(dataLength);

            return new TrueTypeFont(data);
        }
    }
}
