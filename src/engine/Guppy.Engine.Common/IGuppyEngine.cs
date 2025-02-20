using Guppy.Core.Common.Services;

namespace Guppy.Engine.Common
{
    public interface IGuppyEngine : IDisposable
    {
        IGlobalSystemService Systems { get; }

        IGuppyEngine Start();

        T Resolve<T>()
            where T : class;
    }
}