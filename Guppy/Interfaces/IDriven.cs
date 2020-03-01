using Guppy.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IDriven : IFrameable
    {
        FrameableCollection<IDriver> Drivers { get; set; }
    }
}
