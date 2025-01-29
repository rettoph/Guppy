using System.Reflection;
using Guppy.Core.Common.Implementations;

namespace Guppy.Core.Common.Constants
{
    public static class GuppyEnvironmentVariables
    {
        public class Project(string value) : EnvironmentVariable<Project, string>(value)
        {
        }

        public class Company(string value) : EnvironmentVariable<Company, string>(value)
        {
        }

        public class LibraryAssemblies(Assembly[] values) : EnvironmentVariable<LibraryAssemblies, Assembly[]>(values)
        {
        }
        public class EntryAssembly(Assembly values) : EnvironmentVariable<EntryAssembly, Assembly>(values)
        {
        }
    }
}
