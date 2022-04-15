using Guppy.EntityComponent;
using Guppy.EntityComponent.Services;
using Guppy.Gaming;
using Microsoft.Xna.Framework;

namespace Guppy.Example.Library
{
    public class ExampleScene : Scene
    {
        public ExampleScene(IEntityService entities) : base(entities)
        {
        }
    }
}