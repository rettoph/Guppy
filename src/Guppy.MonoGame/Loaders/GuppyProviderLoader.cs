using Guppy.Loaders;
using Guppy.MonoGame.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Loaders
{
    internal sealed class GuppyProviderLoader : IEngineLoader
    {
        public void Load(GuppyEngine engine)
        {
            engine.Guppies.Attach(new DrawableCollection());
            engine.Guppies.Attach(new UpdateableCollection());
        }
    }
}
