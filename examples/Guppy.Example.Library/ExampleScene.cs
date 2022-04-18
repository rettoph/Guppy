using Guppy.EntityComponent;
using Guppy.EntityComponent.Services;
using Guppy.Gaming;
using Guppy.Providers;
using Microsoft.Xna.Framework;

namespace Guppy.Example.Library
{
    public class ExampleScene : Scene
    {
        public ExampleScene(ISettingProvider settings, ITextProvider text, IEntityService entities) : base(entities)
        {
        }
    }
}