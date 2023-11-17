using Guppy.Loaders;
using Guppy.MonoGame.Collections;
using Guppy.MonoGame.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Loaders
{
    internal sealed class GuppyProviderLoader : IGlobalLoader
    {
        public void Load(GuppyEngine engine)
        {
            engine.Guppies.Attach(new List<IGuppyDrawable>());
            engine.Guppies.Attach(new List<IGuppyUpdateable>());
        }
    }
}
