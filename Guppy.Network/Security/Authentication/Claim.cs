using Guppy.Enums;
using Guppy.Network.Security.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Security.Authentication
{
    /// <summary>
    /// A claim represents a simple value belonging to
    /// a user instance.
    /// </summary>
    public class Claim
    {
        public ClaimScope Scope { get; internal set; }
        public String Value { get; internal set; }
    }
}
