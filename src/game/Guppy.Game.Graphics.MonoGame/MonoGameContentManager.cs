using Guppy.Game.Graphics.Common;
using Guppy.Game.Graphics.Common.Enums;
using Microsoft.Xna.Framework.Content;

namespace Guppy.Game.Graphics.MonoGame
{
    public class MonoGameContentManager(ContentManager value) : IContentManager
    {
        public ContentManager Value { get; } = value;

        public GraphicsObjectStatusEnum Status => GraphicsObjectStatusEnum.Implemented;
    }
}
