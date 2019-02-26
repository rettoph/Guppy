using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Demo.Configurations
{
    public class BrickConfiguration
    {
        public String ColorHandle;
        public Byte Health;
        public Boolean Invincible;
        public Boolean Ghost;

        public BrickConfiguration()
        {
            this.Invincible = false;
            this.Ghost = false;
            this.Health = 1;
        }
    }
}
