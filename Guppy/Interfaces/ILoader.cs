using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    /// <summary>
    /// Loaders represent static objects
    /// that contain usable data such as strings,
    /// colors, or entities.
    /// </summary>
    public interface ILoader
    {
        void Load();
    }
}
