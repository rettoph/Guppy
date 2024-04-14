using Guppy.Engine.Common.Services;

namespace Guppy.Engine.Loaders
{
    public interface IAssemblyLoader
    {
        void ConfigureAssemblies(IAssemblyService assemblies);
    }
}
