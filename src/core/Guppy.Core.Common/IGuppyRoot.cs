using System.Collections.ObjectModel;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Services;

namespace Guppy.Core.Common
{
    public interface IGuppyRoot : IGuppyScope
    {
        /// <summary>
        /// All active <see cref="IGuppyScope"/>s, including the current <see cref="MainScope"/>
        /// </summary>
        ReadOnlyCollection<IGuppyScope> Scopes { get; }

        /// <summary>
        /// Envirnoment variables defined at startup
        /// </summary>
        IEnvironmentVariableService EnvironmentVariables { get; }

        /// <summary>
        /// Create a new <see cref="IGuppyScopeBuilder"/> instance.
        /// </summary>
        /// <returns></returns>
        IGuppyScope CreateScope(Action<IGuppyScopeBuilder>? build = null);
    }
}
