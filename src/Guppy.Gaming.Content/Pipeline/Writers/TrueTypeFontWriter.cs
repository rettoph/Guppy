using Guppy.Gaming.Content.Pipeline.Readers;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Content.Pipeline.Writers
{
    [ContentTypeWriter]
    public class TrueTypeFontWriter : ContentTypeWriter<TrueTypeFont>
    {
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(TrueTypeFontReader).AssemblyQualifiedName ?? throw new Exception();
        }

        protected override void Write(ContentWriter output, TrueTypeFont value)
        {
            output.Write(value.Data.Length);
            output.Write(value.Data);
        }
    }
}
