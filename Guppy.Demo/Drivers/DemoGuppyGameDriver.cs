using Guppy.Attributes;
using Guppy.Collections;
using Guppy.Demo.Entities;
using Guppy.Implementations;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Demo.Drivers
{
    [IsDriver(typeof(Game))]
    public class DemoGuppyGameDriver : Driver<Game>
    {
        private EntityCollection _entities;

        public DemoGuppyGameDriver(EntityCollection entities, Game parent) : base(parent)
        {
            _entities = entities;
        }

        protected override void Initialize()
        {
            base.Initialize();

            var entity = _entities.Build<DemoEntity>("entity:demo", e =>
            {

            });
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
