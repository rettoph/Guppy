﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Styling.StylerValues
{
    internal interface IStylerValue
    {
        void Push();
        void Pop();
    }
}