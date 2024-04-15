using Guppy.Core.Common.Services;

namespace Guppy.Engine.Common.Loaders
{
    public interface IAssemblyLoader
    {
        void ConfigureAssemblies(IAssemblyService assemblies);
    }
}
