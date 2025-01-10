using Autofac.Extras.Moq;

namespace Guppy.Tests.Common.Extensions
{
    public static class AutoMockExtensions
    {
        public static Mocker<T> Mocker<T>(this AutoMock mock)
            where T : class => mock.Mock<T>().ToMocker();
    }
}