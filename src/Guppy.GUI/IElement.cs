﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    public interface IElement
    {
        Selector Selector { get; }
        ElementState State { get; }
    }
}
