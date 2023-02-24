﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Providers
{
    public interface IStyleSheetProvider
    {
        IStyleSheet Get(string name);
    }
}
