using Guppy.Configurations;
using Microsoft.Extensions.Logging;

namespace Guppy.Demo
{
    public class DemoLayer : Layer
    {
        protected ILogger logger { get; private set; }

        public DemoLayer(ILogger logger, Scene scene, LayerConfiguration configuration) : base(scene, configuration)
        {
            logger.LogInformation($"Creating new layer...");
        }
    }
}