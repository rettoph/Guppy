﻿using Guppy.Common;
using Guppy.GUI.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Providers
{
    internal sealed class StyleSheetProvider : IStyleSheetProvider
    {
        private Dictionary<string, IStyleSheet> _sheets = new();

        public StyleSheetProvider(ISorted<IStyleSheetLoader> loaders)
        {
            foreach(var loader in loaders)
            {
                loader.Load(this);
            }
        }

        public IStyleSheet Get(string name)
        {
            if(!_sheets.TryGetValue(name, out var sheet))
            {
                sheet = new StyleSheet();
                _sheets.Add(name, sheet);
            }

            return sheet;
        }
    }
}