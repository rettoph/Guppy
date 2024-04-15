using Guppy.Engine.Common;
using Guppy.Core.Resources;
using Microsoft.Xna.Framework;

namespace Guppy.Game.ImGui
{
    internal class NotImplementedImguiBatch : IImguiBatch
    {
        public bool Running => throw new NotImplementedException();

        public void Begin(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void End()
        {
            throw new NotImplementedException();
        }

        public Ref<ImFontPtr> GetFont(Resource<TrueTypeFont> ttf, int size)
        {
            throw new NotImplementedException();
        }
    }
}
