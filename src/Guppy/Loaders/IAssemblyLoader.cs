using Guppy.Common.Providers;

namespace Guppy.Loaders
{
    public interface IAssemblyLoader
    {
        void ConfigureAssemblies(IAssemblyProvider assemblies);
    }
}
