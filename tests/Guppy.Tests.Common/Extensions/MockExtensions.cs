using Moq;

namespace Guppy.Tests.Common.Extensions
{
    public static class MockExtensions
    {
        public static Mocker<TMock> ToMocker<TMock>(this Mock<TMock> mock)
            where TMock : class
        {
            return new()
            {
                Mock = mock
            };
        }
    }
}