using Guppy.EntityComponent;
using Guppy.Gaming.Messages;
using Guppy.Gaming.Services;
using Guppy.Threading;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Components
{
    internal sealed class GameCommandsComponent : Component<Game>, ISubscriber<EntitiesMessage>
    {
        private ICommandService _commands;
        private Game _game;

        public GameCommandsComponent(ICommandService commands)
        {
            _game = null!;
            _commands = commands;
        }

        protected override void Initialize(Game entity)
        {
            _game = entity;

            _commands.Subscribe(this);
        }

        protected override void Uninitilaize()
        {
            _commands.Unsubscribe(this);
        }

        public bool Process(in EntitiesMessage message)
        {
            if(_game.Scenes.Scene is null)
            {
                return false;
            }

            foreach(IEntity entity in _game.Scenes.Scene.Entities)
            {
                Console.WriteLine($"{entity.GetType().GetPrettyName()}:{entity.Id} - {entity}");
            }

            return true;
        }
    }
}
