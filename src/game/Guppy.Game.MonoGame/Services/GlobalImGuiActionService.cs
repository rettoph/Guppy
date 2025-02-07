using Guppy.Core.Common;
using Guppy.Game.ImGui.Common.Enums;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Services
{
    public class GlobalImGuiActionService
    {
        private readonly ActionSequenceGroup<ImGuiSequenceGroupEnum, GameTime> _actions = new(true);

        public void Add(IEnumerable<object> instances)
        {
            this._actions.Add(instances);
        }

        public void Remove(IEnumerable<object> instances)
        {
            this._actions.Remove(instances);

            // I dont think ActionSequenceGroup.Remove works as expected. 
            // TODO: Test
            throw new NotImplementedException();
        }

        public void Draw(GameTime gameTime)
        {
            this._actions.Invoke(gameTime);
        }
    }
}
