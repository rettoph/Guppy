using Guppy.Game.Graphics.Common;
using Microsoft.Xna.Framework.Content;

namespace Guppy.Game.Graphics.MonoGame
{
    public class MonoGameContentManager(ContentManager value) : IContentManager
    {
        public ContentManager Value { get; } = value;
    }
}
