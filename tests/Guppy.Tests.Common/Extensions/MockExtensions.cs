using Moq;

namespace Guppy.Tests.Common.Extensions
{
    public static class MockExtensions
    {
        public static Mocker<T> ToMocker<T>(this Mock<T> mock)
            where T : class
        {
            return new Mocker<T>(mock);
        }
    }
}
