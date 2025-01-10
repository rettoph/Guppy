namespace Guppy.Core.Common.Services
{
    public interface IConfigurationService
    {
        void Configure<T>(T instance);
    }
}