using Guppy.Core.Common.Exceptions;

namespace Guppy.Core.Common
{
    /// <summary>
    /// Global logger meant for system messages.
    /// Probably shouldn't be used directly by
    /// non Guppy libraries
    /// </summary>
    public static class GuppyLogger
    {
        public static GuppyException LogException(string message, Exception ex)
        {
            Console.WriteLine($"{message}\r\n{ex}");
            return new GuppyException(message, ex);
        }
    }
}