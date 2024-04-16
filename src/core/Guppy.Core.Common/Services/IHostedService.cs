namespace Guppy.Core.Common.Services
{
    /// <summary>
    /// Kinda emulates Microsoft.Extensions.Hosting.IHostedService 
    /// The engine implementation is responsible for resolving all
    /// these services at boot time and starting them.
    /// </summary>
    public interface IHostedService
    {
        Task StartAsync(CancellationToken cancellation);
        Task StopAsync(CancellationToken cancellation);
    }
}
