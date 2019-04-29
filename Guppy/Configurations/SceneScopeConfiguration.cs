using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Configurations
{
    /// <summary>
    /// There are scoped instances created via factory
    /// within games (namely the scope's Scene instance)
    /// 
    /// This acts as a simple structure defining the scopes
    /// current settings, allowing factory methods to create
    /// new instances (or load existing instances)
    /// </summary>
    public class SceneScopeConfiguration
    {
        public Scene Scene;
    }
}
