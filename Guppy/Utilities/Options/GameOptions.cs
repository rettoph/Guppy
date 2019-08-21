using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities.Options
{
    /// <summary>
    /// The game configuration class is used within
    /// a guppy service provider instance to track
    /// which game instance is currently created.
    /// 
    /// This makes it easy to ensure that only one game
    /// is build per service provider.
    /// </summary>
    internal class GameOptions
    {
        public Game Instance { get; internal set; }

        /// <summary>
        /// A list of all scene types registered in the service loader.
        /// </summary>
        public HashSet<Type> SceneTypes { get; private set; }

        public GameOptions()
        {
            this.SceneTypes = new HashSet<Type>();
        }
    }
}
