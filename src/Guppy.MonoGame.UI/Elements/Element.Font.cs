using Guppy.MonoGame.UI.Utilities.ImGuiPushManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Elements
{
    public partial class Element
    {
        private ImGuiFontManager? _imGuiFonts;

        public ImGuiFont? Font
        {
            get => _imGuiFonts?.Font;
            set
            {
                if (value is null)
                {
                    this.RemovePushManager(_imGuiFonts);
                    return;
                }

                if (_imGuiFonts is null)
                {
                    _imGuiFonts = this.AddPushManager(new ImGuiFontManager(value));
                    return;
                }

                _imGuiFonts.Font = value;
            }
        }
    }
}
