using Guppy.Core.Common.Services;

namespace Guppy.Engine.Common.Loaders
{
    [Obsolete]
    public interface IAssemblyLoader
    {
        void ConfigureAssemblies(IAssemblyService assemblies);
    }
}
