using Guppy.Common.Services;

namespace Guppy.Loaders
{
    public interface IAssemblyLoader
    {
        void ConfigureAssemblies(IAssemblyService assemblies);
    }
}
