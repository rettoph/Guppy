using System.Reflection;

namespace Guppy.Core.Commands.Common.Contexts
{
    public interface IArgumentContext
    {
        string Name { get; }
        string? Description { get; }
        PropertyInfo PropertyInfo { get; }
    }
}
