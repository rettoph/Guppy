using Guppy.Engine.Common.Enums;

namespace Guppy.Engine.Common.Services
{
    public interface IObjectTextFilterService
    {
        TextFilterResult Filter(object? instance, string input, int maxDepth = 5, int currentDepth = 0, HashSet<object>? tree = null);
    }
}
