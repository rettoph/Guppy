using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Security;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.Layerables
{
    public class Paddle : Positionable
    {
        public const Int32 Height = 15;
        public const Int32 Width = 75;

        public User User { get; set; }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);
        }
    }
}
