using Guppy.Game.Graphics.Common;
using Guppy.Game.Graphics.Common.Enums;
using Microsoft.Xna.Framework.Content;

namespace Guppy.Game.Graphics.NotImplemented
{
    public class NotImplementedContentManager : IContentManager
    {
        public ContentManager Value => throw new NotImplementedException();

        public GraphicsObjectStatusEnum Status => GraphicsObjectStatusEnum.NotImplemented;
    }
}