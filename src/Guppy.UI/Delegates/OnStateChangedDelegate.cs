using Guppy.UI.Enums;
using Guppy.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Delegates
{
    public delegate void OnStateChangedDelegate(IElement sender, ElementState which, Boolean value);
}
