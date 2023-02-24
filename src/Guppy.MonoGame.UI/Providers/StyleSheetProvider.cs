using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Providers
{
    internal sealed class StyleSheetProvider : IStyleSheetProvider
    {
        private Dictionary<string, IStyleSheet> _sheets = new();

        public IStyleSheet Get(string name)
        {
            if(!_sheets.TryGetValue(name, out IStyleSheet? sheet))
            {
                sheet = new StyleSheet()
                {
                    Name = name
                };

                _sheets.Add(name, sheet);
            }

            return sheet;
        }
    }
}
