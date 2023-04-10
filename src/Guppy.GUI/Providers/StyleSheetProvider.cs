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
        private IStyleSheetLoader[] _loaders;

        public StyleSheetProvider(IResourceProvider resources, ISorted<IStyleSheetLoader> loaders)
        {
            _loaders = loaders.ToArray();
            _resources = resources;
            foreach(var loader in _loaders)
            {
                loader.Load(this);
            }
        }

        public IStyleSheet Get(string name)
        {
            if(!_sheets.TryGetValue(name, out var sheet))
            {
                sheet = new StyleSheet(name, _resources);
                _sheets.Add(name, sheet);

                foreach (var loader in _loaders)
                {
                    loader.Configure(sheet);
                }
            }

            return sheet;
        }
    }
}
