using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Enums
{
    /// <summary>
    /// Used to define what scope an object belongs to
    /// in an Initializable object. Mostly seen within
    /// the delegater.
    /// </summary>
    public enum Scope
    {
        Global,
        Instance
    }
}
