using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Configurations
{
    /// <summary>
    /// There are scoped instances created via factory
    /// within games (namely the scope's Game instance)
    /// 
    /// This acts as a simple structure defining the scopes
    /// current settings, allowing factory methods to create
    /// new instances (or load existing instances)
    /// </summary>
    public class GameScopeConfiguration
    {
        public Game Game;

        /// <summary>
        /// Clone another scope's configuration
        /// This is primarily used when creating a
        /// nested scene scope.
        /// </summary>
        /// <param name="gameScopeConfiguration"></param>
        internal void Clone(GameScopeConfiguration gameScopeConfiguration)
        {
            this.Game = gameScopeConfiguration.Game;
        }
    }
}
