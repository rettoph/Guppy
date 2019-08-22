using Microsoft.Extensions.Logging;
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
    public sealed class GameOptions
    {
        public Game Instance { get; internal set; }

        /// <summary>
        /// A list of all creatable scene types registered in the service loader.
        /// </summary>
        public HashSet<Type> SceneTypes { get; private set; }

        /// <summary>
        /// A list of all creatable layer types registered in the service loader
        /// </summary>
        public HashSet<Type> LayerTypes { get; private set; }

        /// <summary>
        /// The current logging level.
        /// 
        /// It is up to the ILogger instance to actually use this value.
        /// </summary>
        public LogLevel LogLevel { get; set; }

        public GameOptions()
        {
            this.SceneTypes = new HashSet<Type>();
            this.LayerTypes = new HashSet<Type>();

            this.LogLevel = LogLevel.Debug;
        }
    }
}
