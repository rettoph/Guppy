using System.Reflection;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Implementations;

namespace Guppy.Core.Common.Constants
{
    public static class GuppyCoreVariables
    {
        public static class Global
        {
            public class Project(string value) : EnvironmentVariable<Project, string>(value)
            {
            }

            public class Company(string value) : EnvironmentVariable<Company, string>(value)
            {
            }
            public class EntryAssembly(Assembly values) : EnvironmentVariable<EntryAssembly, Assembly>(values)
            {
            }
        }

        public static class Scope
        {
            public class ScopeType(GuppyScopeTypeEnum value) : ScopeVariable<ScopeType, GuppyScopeTypeEnum>(value)
            {

            }
        }
    }
}
