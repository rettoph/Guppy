using Guppy.Common;
using Guppy.GUI.Loaders;
using Guppy.Resources.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Providers
{
    internal sealed class StyleSheetProvider : IStyleSheetProvider
    {
        private readonly IResourceProvider _resources;
        private readonly Dictionary<string, IStyleSheet> _sheets = new();

        public StyleSheetProvider(IResourceProvider resources, ISorted<IStyleSheetLoader> loaders)
        {
            _resources = resources;
            foreach(var loader in loaders)
            {
                loader.Load(this);
            }
        }

        public IStyleSheet Get(string name)
        {
            if(!_sheets.TryGetValue(name, out var sheet))
            {
                sheet = new StyleSheet(_resources);
                _sheets.Add(name, sheet);
            }

            return sheet;
        }
    }
}
