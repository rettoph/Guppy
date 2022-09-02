﻿using Guppy.Attributes;
using Guppy.Network.ECS.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.ECS.Attributes
{
    public class WithMasterAuthorizationFilterAttribute : WithFilterAttribute
    {
        public WithMasterAuthorizationFilterAttribute() : base(typeof(MasterAuthorizationFilter))
        {
        }
    }
}
