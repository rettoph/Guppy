using Guppy.EntityComponent;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming
{
    public interface IFrameable : IEntity
    {
        bool Enabled { get; set; }
        bool Visible { get; set; }

        event OnEventDelegate<IFrameable, bool>? OnEnabledChanged;
        event OnEventDelegate<IFrameable, bool>? OnVisibleChanged;

        void Update(GameTime gameTime);

        void Draw(GameTime gameTime);
    }
}
