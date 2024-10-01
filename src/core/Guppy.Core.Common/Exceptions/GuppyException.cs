namespace Guppy.Core.Common.Exceptions
{
    public class GuppyException(string message, Exception inner) : Exception(message, inner)
    {
    }
}
