using Guppy.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;

namespace Guppy.Demo
{
    public class DemoLayer : Layer
    {
        protected ILogger logger { get; private set; }

        public DemoLayer(ILogger logger, Scene scene, LayerConfiguration configuration) : base(scene, configuration)
        {
            logger.LogInformation($"Creating new layer...");
        }

        public override void Draw(GameTime gameTime)
        {
            this.entities.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            this.entities.Update(gameTime);
        }
    }
}