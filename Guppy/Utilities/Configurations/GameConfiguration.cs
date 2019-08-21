using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Configurations
{
    /// <summary>
    /// The game configuration class is used within
    /// a guppy service provider instance to track
    /// which game instance is currently created.
    /// 
    /// This makes it easy to ensure that only one game
    /// is build per service provider.
    /// </summary>
    internal class GameConfiguration
    {
        public Game Instance { get; internal set; }
    }
}
