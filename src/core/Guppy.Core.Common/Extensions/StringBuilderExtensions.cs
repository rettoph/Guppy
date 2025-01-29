using System.Text;

namespace Guppy.Core.Common.Extensions
{
    public static class StringBuilderExtensions
    {
        public static string Flush(this StringBuilder builder)
        {
            string result = builder.ToString();
            builder.Clear();

            return result;
        }
    }
}
