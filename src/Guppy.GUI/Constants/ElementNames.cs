using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Constants
{
    internal sealed class ElementNames
    {
        public const string TextInputLabel = "#textinput_label";
        public const string ScrollBox = "#scrollbox";
        public const string Terminal = "#terminal";
        public const string TerminalOutputContainer = $"{Terminal}#output_container";
        public const string TerminalOutput = $"{Terminal}#output";
        public const string TerminalInput = $"{Terminal}#input";
    }
}
