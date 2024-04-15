namespace Guppy.Core.Common.Exceptions
{
    public class GuppyException : Exception
    {
        public GuppyException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
